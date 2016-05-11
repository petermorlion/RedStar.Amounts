using RedStar.Amounts.StandardUnits;
using Xunit;

namespace RedStar.Amounts.Tests
{
    public class UnitTests
    {
        [Fact]
        public void UnitParseTestWithSimpleUnits()
        {
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);

            Assert.Equal(LengthUnits.Meter, Unit.Parse("m"));
        }

        [Fact]
        public void UnitParseTestWithCalculatedUnits()
        {
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);

            Assert.Equal(LengthUnits.Meter / TimeUnits.Second, Unit.Parse("m/s"));
            Assert.Equal(SpeedUnits.MeterPerSecond, Unit.Parse("m/s"));

            Assert.Equal(MassUnits.KiloGram * 1000, Unit.Parse("1000*Kg"));

            Assert.Equal(VolumeUnits.Meter3 / TimeUnits.Hour, Unit.Parse("m³/h"));

            Assert.Equal(TemperatureUnits.DegreeCelcius / TimeUnits.Second, Unit.Parse("1*°C/s"));
        }

        [Fact]
        public void UnitParseTestWithSillyUnits()
        {
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);

            Assert.Equal(VolumeUnits.Meter3 / TimeUnits.Hour / LengthUnits.Meter, Unit.Parse("m³/h/m"));
            Assert.Equal(VolumeUnits.Meter3 / TimeUnits.Hour / LengthUnits.Meter * MassUnits.KiloGram, Unit.Parse("m³/h/m*Kg"));

            Assert.Equal(TemperatureUnits.DegreeCelcius / TimeUnits.Second, Unit.Parse("1**°C/s"));
            Assert.Equal(TemperatureUnits.DegreeCelcius / TimeUnits.Second, Unit.Parse("1***°C/s"));
        }
    }
}