using ClientAuthorization.Models.Database;
using ClientAuthorization.Services.DTOs.ResponseEntities;

namespace ClientAuthorization.DTOs.ResponseEntities
{
    public class ConfirmationResponse
    {
        public ConfirmationResponse(OperationLog operation)
        {
            Operation = operation;
        }

        public OperationLog Operation { get; set; }
    }
}
