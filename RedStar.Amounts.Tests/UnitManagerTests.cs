using Xunit;

namespace RedStar.Amounts.Tests
{
    public class UnitManagerTests
    {
        [Fact]
        public void TestRegisteringUnit()
        {
            var unit = new Unit("SomeUnit", "SU", new UnitType("SomeUnitType"));
            UnitManager.RegisterUnit(unit);

            var byName = UnitManager.GetUnitByName("SomeUnit");
            Assert.Equal(unit, byName);

            var bySymbol = UnitManager.GetUnitBySymbol("SU");
            Assert.Equal(unit, bySymbol);
        }

        [Fact]
        public void TestRegisteringUnitWithoutBaseUnit()
        {
            var unit = new Unit("SomeUnit", "SU", Unit.None);
            UnitManager.RegisterUnit(unit);

            var byName = UnitManager.GetUnitByName("SomeUnit");
            Assert.Equal(unit, byName);

            var bySymbol = UnitManager.GetUnitBySymbol("SU");
            Assert.Equal(unit, bySymbol);
        }
    }
}