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

        [Fact]
        public void TestGetUnitByNameForUnknownUnit()
        {
            UnknownUnitException thrownException = null;
            try
            {
                UnitManager.GetUnitByName("bla");
            }
            catch (UnknownUnitException e)
            {
                thrownException = e;
            }

            Assert.NotNull(thrownException);
        }

        [Fact]
        public void TestTryGetUnitByNameForUnknownUnit()
        {
            Unit unit;
            var result = UnitManager.TryGetUnitByName("bla", out unit);

            Assert.Null(unit);
            Assert.False(result);
        }

        [Fact]
        public void TestTryGetUnitByNameForKnownUnit()
        {
            var unit = new Unit("SomeUnit", "SU", Unit.None);
            UnitManager.RegisterUnit(unit);

            Unit retrievedUnit;
            var result = UnitManager.TryGetUnitByName("SomeUnit", out retrievedUnit);

            Assert.Equal(unit, retrievedUnit);
            Assert.True(result);
        }
    }
}