using System;

namespace RedStar.Amounts
{
    public static class AmountMath
    {
        public static Amount Max(Amount val1, Amount val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static Amount Min(Amount val1, Amount val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        public static Amount Round(Amount amount, int digits)
        {
            var roundedValue = Math.Round(amount.Value, digits);
            return new Amount(roundedValue, amount.Unit);
        }
    }
}