using System;
using System.Collections.Generic;
using System.Linq;

namespace RedStar.Amounts
{
    public static class Extensions
    {
        /// <summary>Computes the sum of a sequence of Amounts. The units must be convertible to each other.</summary>
        /// <param name="source">A sequence of Amounts to calculate the sum of.</param>
        /// <returns>The sum of the Amounts in the sequence.</returns>
        public static Amount Sum(this IEnumerable<Amount> source)
        {
            return source.Aggregate((x, y) => x + y);
        }

        /// <summary>Computes the sum of a sequence of Amounts that are obtained by transform function on each element of the sequence.
        /// The units must be convertible to each other.</summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The sum of the Amounts in the sequence.</returns>
        public static Amount Sum<T>(this IEnumerable<T> source, Func<T, Amount> selector)
        {
            return source.Any() ? source.Select(selector).Aggregate((x, y) => x + y) : Amount.Zero(Unit.None);
        }

        /// <summary>
        /// Computes the average of a sequence of Amounts. The units must be convertible to each other.
        /// </summary>
        /// <param name="source">A sequence of Amounts to calculate the average of.</param>
        /// <returns>The average of the Amounts in the sequence.</returns>
        public static Amount Average(this IEnumerable<Amount> source)
        {
            var sum = source.Sum();
            var count = source.Count();
            return sum / count;
        }

        /// <summary>
        /// Computes the average of a sequence of Amounts that are obtained by transform function on each element of the sequence.
        /// The units must be convertible to each other.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <returns>The average of the Amounts in the sequence.</returns>
        public static Amount Average<T>(this IEnumerable<T> source, Func<T, Amount> selector)
        {
            var sum = source.Select(selector).Sum();
            var count = source.Count();
            return sum / count;
        }

        /// <summary>
        /// Limits a given Amount to be between the minimum and maximum provided. If the given Amount is lower than the minimum, the minimum will be returned.
        /// If it is higher than the maximum, the maximum will be returned. If it is between the minimum and maximum, the value will be returned unchanged.
        /// The units must be convertible to each other.
        /// </summary>
        public static Amount Limit(this Amount amount, Amount minimum, Amount maximum)
        {
            return AmountMath.Max(minimum, AmountMath.Min(amount, maximum));
        }
    }
}