using RedStar.Amounts.StandardUnits;
using Xunit;

namespace RedStar.Amounts.Tests
{
    public class AmountMathTests
    {
        [Fact]
        public void TestMax()
        {
            var amount1 = new Amount(10, TemperatureUnits.DegreeCelcius);
            var amount2 = new Amount(20, TemperatureUnits.DegreeCelcius);

            Assert.Equal(amount2, AmountMath.Max(amount1, amount2));
            Assert.Equal(amount2, AmountMath.Max(amount2, amount1));
        }

        [Fact]
        public void TestMin()
        {
            var amount1 = new Amount(10, TemperatureUnits.DegreeCelcius);
            var amount2 = new Amount(20, TemperatureUnits.DegreeCelcius);

            Assert.Equal(amount1, AmountMath.Min(amount1, amount2));
            Assert.Equal(amount1, AmountMath.Min(amount2, amount1));
        }

        [Fact]
        public void TestMaxWithDifferentUnits()
        {
            var amount1 = new Amount(10, LengthUnits.MilliMeter);
            var amount2 = new Amount(1, LengthUnits.Meter);

            Assert.Equal(amount2, AmountMath.Max(amount1, amount2));
            Assert.Equal(amount2, AmountMath.Max(amount2, amount1));
        }

        [Fact]
        public void TestMinWithDifferentUnits()
        {
            var amount1 = new Amount(10, LengthUnits.MilliMeter);
            var amount2 = new Amount(1, LengthUnits.Meter);

            Assert.Equal(amount1, AmountMath.Min(amount1, amount2));
            Assert.Equal(amount1, AmountMath.Min(amount2, amount1));
        }

        [Fact]
        public void TestRound()
        {
            var amount = new Amount(14.016, LengthUnits.CentiMeter);
            Assert.Equal(new Amount(14, LengthUnits.CentiMeter), AmountMath.Round(amount, 0));
            Assert.Equal(new Amount(14.0, LengthUnits.CentiMeter), AmountMath.Round(amount, 1));
            Assert.Equal(new Amount(14.02, LengthUnits.CentiMeter), AmountMath.Round(amount, 2));
            Assert.Equal(new Amount(14.016, LengthUnits.CentiMeter), AmountMath.Round(amount, 3));
        }

        [Fact]
        public void TestAbs()
        {
            var positiveAmount = new Amount(4.2, LengthUnits.CentiMeter);
            Assert.Equal(new Amount(4.2, LengthUnits.CentiMeter), AmountMath.Abs(positiveAmount));

            var negativeAmount = new Amount(-1.8, LengthUnits.CentiMeter);
            Assert.Equal(new Amount(1.8, LengthUnits.CentiMeter), AmountMath.Abs(negativeAmount));

            var zeroAmount = new Amount(0, LengthUnits.CentiMeter);
            Assert.Equal(new Amount(0, LengthUnits.CentiMeter), AmountMath.Abs(zeroAmount));
        }

        [Fact]
        public void TestAverage()
        {
            var amount1 = new Amount(10, TemperatureUnits.DegreeCelcius);
            var amount2 = new Amount(20, TemperatureUnits.DegreeCelcius);
            var amount3 = new Amount(25, TemperatureUnits.DegreeCelcius);
            var amount4 = new Amount(18, TemperatureUnits.DegreeCelcius);

            var list = new[] {amount1, amount2, amount3, amount4};

            Assert.Equal(new Amount(18.25, TemperatureUnits.DegreeCelcius), list.Average());
        }
    }
}