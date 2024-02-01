using System;
using System.Collections.Generic;

namespace ClientAuthorization.Models.Database
{
    public partial class OperationPending
    {
        public int Id { get; set; }
        public int OperationId { get; set; }
        public DateOnly ExpiresAt { get; set; }

        public virtual OperationLog Operation { get; set; } = null!;
    }
}
