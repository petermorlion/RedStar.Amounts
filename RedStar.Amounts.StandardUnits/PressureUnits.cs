namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class PressureUnits
    {
        public static readonly Unit Pascal = new Unit("pascal", "Pa", ForceUnits.Newton * LengthUnits.Meter.Power(-2));
        public static readonly Unit HectoPascal = new Unit("hectopascal", "hPa", 100.0 * Pascal);
        public static readonly Unit KiloPascal = new Unit("kilopascal", "KPa", 1000.0 * Pascal);
        public static readonly Unit Bar = new Unit("bar", "bar", 100000.0 * Pascal);
        public static readonly Unit MilliBar = new Unit("millibar", "mbar", 0.001 * Bar);
        public static readonly Unit Atmosphere = new Unit("atmosphere", "atm", 101325.0 * Pascal);
    }
}