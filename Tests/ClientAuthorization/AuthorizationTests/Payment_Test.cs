using ClientAuthorization.DTOs.RequestEntities;
using Newtonsoft.Json.Linq;


namespace Tests.ClientAuthorization.AuthorizationTests
{
    public class Payment_Test : ClientAuthorizationTestsBase
    {
        
        [Theory]
        [MemberData(nameof(OK_Authorized_Dataset))]
        public async Task AuthorizationOk_Authorized(AuthorizationRequest request)
        {
            var response = await _authorizationController.Payment(request);
            Assert.True(response.Authorized);
        }

        [Theory]
        [MemberData(nameof(OK_Denied_Dataset))]
        public async Task AuthorizationOk_Denied(AuthorizationRequest request)
        {
            var response = await _authorizationController.Payment(request);
            Assert.False(response.Authorized);
        }
        public static IEnumerable<object[]> OK_Authorized_Dataset()
        {
            return GetDataSet("OK_Authorized.json");
        }
        public static IEnumerable<object[]> OK_Denied_Dataset()
        {
            return GetDataSet("OK_Denied.json");
        }
        public static IEnumerable<object[]> GetDataSet(string filename)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/Dataset/ClientAuthorization/Authorization/", filename);
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json);
            var requests = jobject["data"]?.ToObject<IEnumerable<AuthorizationRequest>>();
            foreach (var request in requests ?? Enumerable.Empty<AuthorizationRequest>())
            {
                yield return new[] { request };
            }
        }
    }
}
