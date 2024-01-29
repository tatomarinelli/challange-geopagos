using System.ComponentModel.DataAnnotations;

namespace ClientAuthorization.DTOs.RequestEntities
{
    public record AuthorizationRequest
    {
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
