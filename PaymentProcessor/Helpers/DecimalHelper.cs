using System.Runtime.CompilerServices;

namespace PaymentProcessor.Helpers
{
    public static class DecimalHelper
    {
        // Idea taken from de C# 7 official implementation: 
        // https://github.com/dotnet/runtime/blob/7a60900cd1be3361773237d0d0d0293146db5547/src/libraries/System.Private.CoreLib/src/System/Decimal.cs#L1413
        public static bool IsInteger(this decimal value) => value == Decimal.Truncate(value);
    }
}
