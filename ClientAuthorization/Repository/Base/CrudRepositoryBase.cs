using System.Linq.Expressions;
using System.Data;
using ClientAuthorization.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace ClientAuthorization.Repository.Base
{
    #region Interface CRUD
    public interface ICrudRepositoryBase<T> where T : class
    {
        void Create(T entity);

        void Create(List<T> entities);

        void Update(T entity);

        void Delete(T entity);

        T GetById(object id);

        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression, Expression<Func<T, object>> includes);
    }

    #endregion

    #region Repository CRUD
    public abstract class CrudRepositoryBase<T> : ICrudRepositoryBase<T> where T : class
    {
        public readonly geopagos_dbContext context;
        protected ILogger<CrudRepositoryBase<T>> _logger;

        public CrudRepositoryBase() { }
        public CrudRepositoryBase(geopagos_dbContext db, ILogger<CrudRepositoryBase<T>> logger)
        {
            context = db;
            _logger = logger;
        }

        public virtual void Create(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError("Create", ex);
                throw;
            }
        }

        public virtual void Create(List<T> entities)
        {
            try
            {
                context.Set<T>().AddRange(entities);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError("Create List", ex);
                throw;
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                context.Set<T>().Remove(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError("Delete", ex);
                throw;
            }
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression, 
                                           Expression<Func<T, object>> includes)
        {
            try
            {
                return context.Set<T>().Where(expression).Include(includes);
            }
            catch (Exception ex)
            {
                LogError("Find", ex);
                throw;
            }
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            try
            {
                return context.Set<T>().Where(expression);
            }
            catch (Exception ex)
            {
                LogError("Find", ex);
                throw;
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                LogError("GetAll", ex);
                throw;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                context.Set<T>().Update(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogError("Update", ex);
                throw;
            }
        }

        public virtual T GetById(object id)
        {
            try
            {
                return context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                LogError("GetById", ex);
                throw;
            }
        }

        private void LogError(string method, Exception ex)
        {
            string inner = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
            _logger.LogError($"Error de Base de Datos metodo [{method}],ERROR: [{ex.Message}], STACK: [{ex.StackTrace}], [{inner}] ");
        }
    }
    #endregion
}