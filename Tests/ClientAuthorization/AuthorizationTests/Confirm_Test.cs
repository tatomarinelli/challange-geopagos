
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.Models.Database;
using Newtonsoft.Json.Linq;
using static ClientAuthorization.Models.Database.DatabaseEnums;

namespace Tests.ClientAuthorization.AuthorizationTests
{
    public class Confirm_Test : ClientAuthorizationTestsBase
    {
        [Theory]
        [MemberData(nameof(OK_Confirm_Dataset))]
        public async Task<ConfirmationResponse?> Confirm_OK(AuthorizationRequest requestToAuthorize)
        {
            var response = await _authorizationController.Payment(requestToAuthorize);
            ConfirmationResponse confirmed = null;
            if (response.Authorized)
            {
                confirmed = _authorizationController.Confirm(response.OperationId);
            }

            Assert.True(confirmed?.Operation?.OperationStatus == OperationStatusEnum.AUT.ToString());
            return confirmed;
        }

        [Theory]
        [MemberData(nameof(NotFound_Confirm_Dataset))]
        public ConfirmationResponse? Confirm_NotFound(string id)
        {
            Assert.ThrowsAny<Exception>(() => _authorizationController.Confirm(id));
            return null;
        }

        public static IEnumerable<object[]> OK_Confirm_Dataset()
        {
            return GetDataSet<AuthorizationRequest>("OK_Confirm.json");
        }
        public static IEnumerable<object[]> NotFound_Confirm_Dataset()
        {
            return GetDataSet<string>("NotFound_Confirm.json");
        }
        public static IEnumerable<T[]> GetDataSet<T>(string filename)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/Dataset/ClientAuthorization/Authorization/", filename);
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json);
            var requests = jobject["data"]?.ToObject<IEnumerable<T>>();
            foreach (var request in requests ?? Enumerable.Empty<T>())
            {
                yield return new[] { request };
            }
        }
    }
}
