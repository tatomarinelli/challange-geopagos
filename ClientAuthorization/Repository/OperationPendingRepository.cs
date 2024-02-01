using ClientAuthorization.Models.Database;
using ClientAuthorization.Repository.Base;

namespace ClientAuthorization.Repository.Interface
{
    public class OperationPendingRepository : CrudRepositoryBase<OperationPending>, IOperationPendingRepository
    {
        public OperationPendingRepository() { }
        public OperationPendingRepository(geopagos_dbContext db, ILogger<CrudRepositoryBase<OperationPending>> logger) : base(db, logger)
        {
        }
    }
}
