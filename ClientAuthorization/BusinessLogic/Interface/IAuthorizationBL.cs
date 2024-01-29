﻿using ClientAuthorization.DTOs.RequestEntities;
using ClientAuthorization.DTOs.ResponseEntities;
using ClientAuthorization.Modules;

namespace ClientAuthorization.BusinessLogic.Interface
{
    public interface IAuthorizationBL : IModule
    {
        Task<AuthorizationResponse> Payment(AuthorizationRequest request);
        void Reverse();
        void Return();
    }
}
