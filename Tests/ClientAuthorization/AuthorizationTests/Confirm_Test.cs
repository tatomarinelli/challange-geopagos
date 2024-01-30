
using ClientAuthorization.DTOs.RequestEntities;
using Newtonsoft.Json.Linq;

namespace Tests.ClientAuthorization.AuthorizationTests
{
    public class Confirm_Test : ClientAuthorizationTestsBase
    {
        [Theory]
        [MemberData(nameof(NotFound_Confirm_Dataset))]
        public async Task Confirm_NotFound(string id)
        {
            await Assert.ThrowsAnyAsync<Exception>(() => _authorizationController.Confirm(id));
        }

        public static IEnumerable<object[]> NotFound_Confirm_Dataset()
        {
            return GetDataSet("NotFound_Confirm.json");
        }
        public static IEnumerable<object[]> GetDataSet(string filename)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/ClientAuthorization/AuthorizationTests/Dataset", filename);
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json);
            var requests = jobject["data"]?.ToObject<IEnumerable<string>>();
            foreach (var request in requests ?? Enumerable.Empty<string>())
            {
                yield return new[] { request };
            }
        }
    }
}
