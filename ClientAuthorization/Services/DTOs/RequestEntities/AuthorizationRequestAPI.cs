using ClientAuthorization.DTOs.RequestEntities;
using System.ComponentModel.DataAnnotations;

namespace ClientAuthorization.Services.DTOs.RequestEntities
{
    public record AuthorizationRequestAPI
    {
        public AuthorizationRequestAPI(AuthorizationRequest request)
        {
            this.ClientID = request.ClientID;
            this.ClientType = request.ClientType;

            this.CardData = new CardData()
            {
                HolderName = request.CardData.HolderName,
                Type = request.CardData.Type,
                Number = request.CardData.Number,
                CVC = request.CardData.CVC,
                ExpirationDate = request.CardData.ExpirationDate,
            };
            this.TransactionData = new TransactionData()
            {
                Amount = request.TransactionData.Amount,
                Date = request.TransactionData.Date,
            };
        }

        [Required]
        public string? ClientID { get; set; }
        [Required]
        public string? ClientType { get; set; }
        [Required]
        public CardData? CardData { get; set; }
        [Required]
        public TransactionData TransactionData { get; set; }


    }
    public class TransactionData
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
    public class CardData
    {
        [Required]
        public string? HolderName { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public string? Number { get; set; }
        [Required]
        public string? CVC { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }

}
