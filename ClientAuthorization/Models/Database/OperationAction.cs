using System;
using System.Collections.Generic;

namespace ClientAuthorization.Models.Database
{
    public partial class OperationAction
    {
        public string Action { get; set; } = null!;
        public string? Description { get; set; }
    }
}
