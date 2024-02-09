using Newtonsoft.Json.Linq;
using PaymentProcessor.BusinessLogic;
using PaymentProcessor.BusinessLogic.Interface;
using PaymentProcessor.Controllers;
using PaymentProcessor.DTOs.RequestEntities;

namespace Tests.PaymentProcessor
{
    public class AuthorizationTest
    {
        
        private const string Filename = "OK_Authorized";
        private readonly PaymentController _paymentController;
        private readonly IPaymentAuthorizationBL _authorizationBL;
        public AuthorizationTest()
        {
            _authorizationBL = new PaymentAuthorizationBL();
            _paymentController = new PaymentController(_authorizationBL);
        }
        
        [Theory]
        [MemberData(nameof(OK_Authorized_Dataset))]
        public void AuthorizationOk_Authorized(PaymentAuthorizationRequest request)
        {
            var response = _paymentController.Authorize(request);
            Assert.True(response.Authorized);
        }

        [Theory]
        [MemberData(nameof(OK_Denied_Dataset))]
        public void AuthorizationOk_Denied(PaymentAuthorizationRequest request)
        {
            var response = _paymentController.Authorize(request);
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
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/Dataset/PaymentProcessor/", filename);
            var json = File.ReadAllText(filePath);
            var jobject = JObject.Parse(json);
            var requests = jobject["data"]?.ToObject<IEnumerable<PaymentAuthorizationRequest>>();
            foreach (var request in requests ?? Enumerable.Empty<PaymentAuthorizationRequest>())
            {
                yield return new[] { request };
            }
        }
    }
}