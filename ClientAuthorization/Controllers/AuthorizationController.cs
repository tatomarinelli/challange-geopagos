using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientAuthorization.BusinessLogic.Interface;
using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.BusinessLogic;

namespace ClientAuthorization.Controllers
{
    [Route("v1/Client/Authorization/Request/[controller]/[action]")]
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
        public async Task<IActionResult> Confirm([FromBody] string id)
        {
            await _AuthorizationBL.Confirm(id);
            return Ok(200);
        }
        /*
        [HttpPost]
        public void Reverse() { }

        [HttpPost]
        public void Return() { }*/
    }
}
