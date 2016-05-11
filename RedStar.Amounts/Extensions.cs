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
    }
}