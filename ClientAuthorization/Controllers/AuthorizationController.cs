using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;

namespace ClientAuthorization.Controllers
{
    [Route("v1/Client/Authorization/Request/[controller]")]
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
        /*
        [HttpPost]
        public void Reverse() { }

        [HttpPost]
        public void Return() { }*/
    }
}
