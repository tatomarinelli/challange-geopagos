using ClientAuthorization.DTOs.RequestEntities;
using Microsoft.VisualBasic;
using static ClientAuthorization.Models.Database.DatabaseEnums;

namespace ClientAuthorization.Models.Database.Mappers
{
    public static class OperationLogMapper
    {
        public static OperationLog Map(AuthorizationRequest request, OperationStatusEnum status, OperationActionEnum action)
        {

            return new OperationLog() 
            {
                ClientId = request.ClientID,
                ClientType = request.ClientType,
                CardHolderName = request.CardData.HolderName,
                CardType = request.CardData.Type,
                CardNumber = request.CardData.Number,
                CardCvc = request.CardData.CVC,
                // CardExpirationDate = request.CardData.ExpirationDate
                TransactionAmount = request.TransactionData.Amount,
                //TransactionDate = request.TransactionData.Date
                OperationStatus = status.ToString(),
                //LastModification = DateTime.Now(),
                OperationAction = action.ToString()
            };
        }
    }
}
