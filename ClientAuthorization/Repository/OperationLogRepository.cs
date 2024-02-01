using ClientAuthorization.Models.Database;
using ClientAuthorization.Repository.Base;

namespace ClientAuthorization.Repository.Interface
{
    public class OperationLogRepository : CrudRepositoryBase<OperationLog>, IOperationLogRepository
    {
        public OperationLogRepository() { } 
        public OperationLogRepository(geopagos_dbContext db, ILogger<CrudRepositoryBase<OperationLog>> logger) : base(db, logger)
        {
        }
    }
}
