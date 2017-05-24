using Xunit;

namespace RedStar.Amounts.StandardUnits.Tests
{
    public class RelativeUnitsTests
    {
        [Fact]
        public void WithPercentage_MultiplyingShouldDivide()
        {
            var percentage = new Amount(50, RelativeUnits.Percentage);
            var amount = new Amount(40, LengthUnits.Meter);

            var result = amount*percentage;

            Assert.Equal(new Amount(20, LengthUnits.Meter), result);
        }

        [Fact]
        public void WithAbsolute_MultiplyingShouldDivide()
        {
            var percentage = new Amount(0.5, RelativeUnits.Absolute);
            var amount = new Amount(40, LengthUnits.Meter);

            var result = amount * percentage;

            Assert.Equal(new Amount(20, LengthUnits.Meter), result);
        }
    }
}
