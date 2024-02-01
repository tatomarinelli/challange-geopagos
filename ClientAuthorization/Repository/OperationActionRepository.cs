using ClientAuthorization.Models.Database;
using ClientAuthorization.Repository.Base;

namespace ClientAuthorization.Repository.Interface
{
    public class OperationActionRepository : CrudRepositoryBase<OperationAction>, IOperationActionRepository
    {
        public OperationActionRepository() { }
        public OperationActionRepository(geopagos_dbContext db, ILogger<CrudRepositoryBase<OperationAction>> logger) : base(db, logger)
        {
        }
    }
}
