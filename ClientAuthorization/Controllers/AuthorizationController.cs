using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.BusinessLogic;
using ClientAuthorization.Models.Database;
using Newtonsoft.Json;

namespace ClientAuthorization.Controllers
{
    [Route("v1/Client/[controller]/[action]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthorizationBL _AuthorizationBL;

        public AuthorizationController(IAuthorizationBL AuthorizationBL)
        {
            _AuthorizationBL = AuthorizationBL;
        }

        [HttpPost]
        public async Task<AuthorizationResponse> Payment([FromBody] AuthorizationRequest request)
        {
            return await _AuthorizationBL.Payment(request);
        }

        [HttpPost]
        public ConfirmationResponse Confirm([FromBody] string id)
        {
            return _AuthorizationBL.Confirm(id);
        }

        [HttpGet]
        public List<PendingOperationResponse> GetPendingOperations()
        {
            return _AuthorizationBL.GetPendingOperations();
        }
        /*
        [HttpPost]
        public void Reverse() { }

        [HttpPost]
        public void Return() { }*/
    }
}
