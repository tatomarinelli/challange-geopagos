﻿using ClientAuthorization.Models.Database;
using ClientAuthorization.Modules;
using ClientAuthorization.Repository.Base;

namespace ClientAuthorization.Repository.Interface
{
    public interface IOperationStatusRepository : ICrudRepositoryBase<OperationStatus>, IModule
    {
    }
}
