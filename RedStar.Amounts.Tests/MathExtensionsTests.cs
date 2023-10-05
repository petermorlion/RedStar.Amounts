using RedStar.Amounts.StandardUnits;
using Xunit;

namespace RedStar.Amounts.Tests
{
    public class MathExtensionsTests
    {
        private class MyClass
        {
            public Amount MyAmount { get; set; }
        }

        [Fact]
        public void SumTest()
        {
            var a = new Amount(3000.0, LengthUnits.Meter);
            var b = new Amount(3500.0, LengthUnits.Meter);
            var c = new Amount(1000.0, LengthUnits.Meter);
            var amounts = new[] { a, b, c };

            var expected = new Amount(7500.0, LengthUnits.Meter);

            var sum = amounts.Sum();

            Assert.Equal(expected, sum);
        }

        [Fact]
        public void SumWithSelectorTest()
        {
            var a = new MyClass { MyAmount = new Amount(3000.0, LengthUnits.Meter) };
            var b = new MyClass { MyAmount = new Amount(3500.0, LengthUnits.Meter) };
            var c = new MyClass { MyAmount = new Amount(1000.0, LengthUnits.Meter) };
            var amounts = new[] { a, b, c };

            var expected = new Amount(7500.0, LengthUnits.Meter);

            var sum = amounts.Sum(x => x.MyAmount);

            Assert.Equal(expected, sum);
        }

        [Fact]
        public void TestAverage()
        {
            var amount1 = new Amount(10, TemperatureUnits.DegreeCelcius);
            var amount2 = new Amount(20, TemperatureUnits.DegreeCelcius);
            var amount3 = new Amount(25, TemperatureUnits.DegreeCelcius);
            var amount4 = new Amount(18, TemperatureUnits.DegreeCelcius);

            var list = new[] { amount1, amount2, amount3, amount4 };

            Assert.Equal(new Amount(18.25, TemperatureUnits.DegreeCelcius), list.Average());
        }

        [Fact]
        public void TestLimit()
        {
            var tooLowAmount = new Amount(10, TemperatureUnits.DegreeCelcius);
            var tooHighAmount = new Amount(100, TemperatureUnits.DegreeCelcius);
            var inBetweenAmount = new Amount(50, TemperatureUnits.DegreeCelcius);

            var minimum = new Amount(40, TemperatureUnits.DegreeCelcius);
            var maximum = new Amount(60, TemperatureUnits.DegreeCelcius);

            Assert.Equal(new Amount(40, TemperatureUnits.DegreeCelcius), tooLowAmount.Limit(minimum, maximum));
            Assert.Equal(new Amount(60, TemperatureUnits.DegreeCelcius), tooHighAmount.Limit(minimum, maximum));
            Assert.Equal(new Amount(50, TemperatureUnits.DegreeCelcius), inBetweenAmount.Limit(minimum, maximum));
        }

        [Fact]
        public void Average_WithSelector_MustReturnCorrectAverage()
        {
            var object1 = new { Amount = new Amount(3, LengthUnits.Meter) };
            var object2 = new { Amount = new Amount(14, LengthUnits.Meter) };
            var list = new[] {object1, object2};

            var result = list.Average(x => x.Amount);

            Assert.Equal(new Amount(8.5, LengthUnits.Meter), result);
        }
    }
}