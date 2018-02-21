using System;

namespace RedStar.Amounts
{
    public static class AmountMath
    {
        /// <summary>
        /// Returns the larger of two Amounts. The Units of the Amounts must be convertible to one another.
        /// </summary>
        /// <param name="val1">The first Amount to compare.</param>
        /// <param name="val2">The second Amount to compare.</param>
        /// <returns>The larger of the two Amounts.</returns>
        public static Amount Max(Amount val1, Amount val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        /// <summary>
        /// Returns the smaller of two Amounts. The Units of the Amounts must be convertible to one another.
        /// </summary>
        /// <param name="val1">The first Amount to compare.</param>
        /// <param name="val2">The second Amount to compare.</param>
        /// <returns>The smaller of the two Amounts.</returns>
        public static Amount Min(Amount val1, Amount val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        /// <summary>Rounds the value of an Amount to a specified number of fractional digits.</summary>
        /// <param name="amount">An Amount to be rounded.</param>
        /// <param name="digits">The number of fractional digits in the return value.</param>
        /// <returns>A new Amount with a value nearest to <paramref name="amount">amount</paramref> that contains a number of fractional digits equal to <paramref name="digits">digits</paramref>.</returns>
        public static Amount Round(Amount amount, int digits)
        {
            var roundedValue = Math.Round(amount.Value, digits);
            return new Amount(roundedValue, amount.Unit);
        }

        /// <summary>Returns the absolute value of an Amount.</summary>
        /// <param name="amount">An Amount to get the absolute value of.</param>
        /// <returns>An Amount with a value x, such that 0 ≤ x ≤<see cref="F:System.Double.MaxValue"></see>.</returns>
        public static Amount Abs(Amount amount)
        {
            return new Amount(Math.Abs(amount.Value), amount.Unit);
        }
    }
}