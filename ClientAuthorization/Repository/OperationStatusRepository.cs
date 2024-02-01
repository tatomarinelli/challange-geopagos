using ClientAuthorization.Models.Database;
using ClientAuthorization.Repository.Base;

namespace ClientAuthorization.Repository.Interface
{
    public class OperationStatusRepository : CrudRepositoryBase<OperationStatus>, IOperationStatusRepository
    {
        public OperationStatusRepository() { }
        public OperationStatusRepository(geopagos_dbContext db, ILogger<CrudRepositoryBase<OperationStatus>> logger) : base(db, logger)
        {
        }
    }
}
