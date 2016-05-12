using System;
using System.Collections.Generic;
using System.Linq;

namespace RedStar.Amounts
{
    public static class Extensions
    {
        public static Amount Sum(this IEnumerable<Amount> source)
        {
            return source.Aggregate((x, y) => x + y);
        }

        public static Amount Sum<T>(this IEnumerable<T> source, Func<T, Amount> selector)
        {
            return source.Any() ? source.Select(selector).Aggregate((x, y) => x + y) : Amount.Zero(Unit.None);
        }

        public static Amount Average(this IEnumerable<Amount> source)
        {
            var sum = source.Sum();
            var count = source.Count();
            return sum / count;
        }

        /// <summary>
        /// Limits a given Amount to be between the minimum and maximum provided. If the given Amount is lower than the minimum, the minimum will be returned.
        /// If it is higher than the maximum, the maximum will be returned. If it is between the minimum and maximum, the value will be returned unchanged.
        /// </summary>
        public static Amount Limit(this Amount amount, Amount minimum, Amount maximum)
        {
            return AmountMath.Max(minimum, AmountMath.Min(amount, maximum));
        }
    }
}