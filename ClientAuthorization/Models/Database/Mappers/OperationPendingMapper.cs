using ClientAuthorization.DTOs.RequestEntities;
using Microsoft.VisualBasic;
using static ClientAuthorization.Models.Database.DatabaseEnums;

namespace ClientAuthorization.Models.Database.Mappers
{
    public static class OperationPendingMapper
    {
        public static OperationPending Map(int id)
        { 
            return new OperationPending()
            {
                OperationId = id
            };
        }
    }
}
