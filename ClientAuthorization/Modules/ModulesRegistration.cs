using System.Reflection;

namespace ClientAuthorization.Modules
{
    // Main idea taken from https://timdeschryver.dev/blog/maybe-its-time-to-rethink-our-project-structure-with-dot-net-6#an-api-with-controllers
    // To simplify the implementation I automated the AddScoped process
    // Every class with IModule interface will be added for DI in the Program.cs.
    public interface IModule
    {
        //IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }

    public static class ModuleExtensions
    {

        // this could also be added into the DI container
        static readonly List<IModule> registeredModules = new List<IModule>();

        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            // TODO: Refactor.

            List<string> namespaceAddScope = new();
            // Get classes with IModule interface
            var modules = DiscoverModules();

            // Find namespaces of modules to register
            foreach (var module in modules)
            {
               namespaceAddScope.Add(module.GetType().Namespace);
            }

            // Get types
            var types = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.IsClass && namespaceAddScope.Contains(t.Namespace)
                        select t;

            // Get interfaces and add to scoped. The interfaces must have the same name as the class.
            // IClass - Class
            types.ToList().ForEach(typeClase =>
            {
                var interfaceForClass = typeClase.GetInterface("I" + typeClase.Name);
                if (interfaceForClass != null)
                    services.AddScoped(interfaceForClass, typeClase);
            });

            return services;
        }

        /*public static WebApplication MapEndpoints(this WebApplication app)
        {
            foreach (var module in registeredModules)
            {
                module.MapEndpoints(app);
            }
            return app;
        }*/

        private static IEnumerable<IModule> DiscoverModules()
        {
            return typeof(IModule).Assembly
                .GetTypes()
                .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
                .Select(Activator.CreateInstance)
                .Cast<IModule>();
        }
    }
}
