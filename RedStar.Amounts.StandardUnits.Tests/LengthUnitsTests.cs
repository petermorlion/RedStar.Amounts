using Xunit;

namespace RedStar.Amounts.StandardUnits.Tests
{
    public class LengthUnitsTests
    {
        [Fact]
        public void MetersTest()
        {
            var a = 3.5.Meters();

            var expected = new Amount(3.5, LengthUnits.Meter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void PicoMetersTest()
        {
            var a = 3.5.PicoMeters();

            var expected = new Amount(3.5, LengthUnits.PicoMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void NanoMetersTest()
        {
            var a = 3.5.NanoMeters();

            var expected = new Amount(3.5, LengthUnits.NanoMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void MicroMetersTest()
        {
            var a = 3.5.MicroMeters();

            var expected = new Amount(3.5, LengthUnits.MicroMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void MilliMetersTest()
        {
            var a = 3.5.MilliMeters();

            var expected = new Amount(3.5, LengthUnits.MilliMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void CentiMetersTest()
        {
            var a = 3.5.CentiMeters();

            var expected = new Amount(3.5, LengthUnits.CentiMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void DeciMetersTest()
        {
            var a = 3.5.DeciMeters();

            var expected = new Amount(3.5, LengthUnits.DeciMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void DecaMetersTest()
        {
            var a = 3.5.DecaMeters();

            var expected = new Amount(3.5, LengthUnits.DecaMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void HectoMetersTest()
        {
            var a = 3.5.HectoMeters();

            var expected = new Amount(3.5, LengthUnits.HectoMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void KiloMetersTest()
        {
            var a = 3.5.KiloMeters();

            var expected = new Amount(3.5, LengthUnits.KiloMeter);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void InchesTest()
        {
            var a = 3.5.Inches();

            var expected = new Amount(3.5, LengthUnits.Inch);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void FeetTest()
        {
            var a = 3.5.Feet();

            var expected = new Amount(3.5, LengthUnits.Foot);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void YardsTest()
        {
            var a = 3.5.Yards();

            var expected = new Amount(3.5, LengthUnits.Yard);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void MilesTest()
        {
            var a = 3.5.Miles();

            var expected = new Amount(3.5, LengthUnits.Mile);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void NauticalMilesTest()
        {
            var a = 3.5.NauticalMiles();

            var expected = new Amount(3.5, LengthUnits.NauticalMile);

            Assert.Equal(expected, a);
        }

        [Fact]
        public void LightYearsTest()
        {
            var a = 3.5.LightYears();

            var expected = new Amount(3.5, LengthUnits.LightYear);

            Assert.Equal(expected, a);
        }
    }
}
