using System;
using System.Collections.Generic;

namespace ClientAuthorization.Models.Database
{
    public partial class OperationStatus
    {
        public OperationStatus()
        {
            OperationLogs = new HashSet<OperationLog>();
        }

        public string Status { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<OperationLog> OperationLogs { get; set; }
    }
}
