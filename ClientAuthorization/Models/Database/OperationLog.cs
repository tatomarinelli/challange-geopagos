using System;
using System.Collections.Generic;

namespace ClientAuthorization.Models.Database
{
    public partial class OperationLog
    {
        public OperationLog()
        {
            OperationPendings = new HashSet<OperationPending>();
        }

        public int OperationId { get; set; }
        public string? ClientId { get; set; }
        public string? ClientType { get; set; }
        public string? CardHolderName { get; set; }
        public string? CardType { get; set; }
        public string? CardNumber { get; set; }
        public string? CardCvc { get; set; }
        public string? CardExpirationDate { get; set; }
        public decimal? TransactionAmount { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public string? OperationStatus { get; set; }
        public DateOnly? LastModification { get; set; }
        public string? OperationAction { get; set; }

        public virtual OperationStatus? OperationStatusNavigation { get; set; }
        public virtual ICollection<OperationPending> OperationPendings { get; set; }
    }
}
