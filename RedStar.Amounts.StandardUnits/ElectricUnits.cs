namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class ElectricUnits
    {
        public static readonly Unit Ampere = new Unit("ampere", "A", SIUnitTypes.ElectricCurrent);
        public static readonly Unit MilliAmpere = new Unit("milliampere", "mA", 0.001 * Ampere);

        public static readonly Unit Coulomb = new Unit("coulomb", "C", TimeUnits.Second * Ampere);
        public static readonly Unit Volt = new Unit("volt", "V", EnergyUnits.Watt / Ampere);
        public static readonly Unit Ohm = new Unit("ohm", "Ω", Volt / Ampere);
        public static readonly Unit Farad = new Unit("farad", "F", Coulomb / Volt);
    }
}