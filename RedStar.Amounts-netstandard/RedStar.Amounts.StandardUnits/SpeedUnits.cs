namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class SpeedUnits
    {
        public static readonly Unit MeterPerSecond = new Unit("meter/second", "m/s", LengthUnits.Meter / TimeUnits.Second);
        public static readonly Unit KilometerPerHour = new Unit("kilometer/hour", "km/h", LengthUnits.KiloMeter / TimeUnits.Hour);
        public static readonly Unit MilePerHour = new Unit("mile/hour", "mi/h", LengthUnits.Mile / TimeUnits.Hour);
        public static readonly Unit Knot = new Unit("knot", "kn", 1.852 * SpeedUnits.KilometerPerHour);
    }
}