namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class TimeUnits
    {
        public static readonly Unit Second = new Unit("second", "s", SIUnitTypes.Time);
        public static readonly Unit MicroSecond = new Unit("microsecond", "μs", 0.000001 * Second);
        public static readonly Unit MilliSecond = new Unit("millisecond", "ms", 0.001 * Second);
        public static readonly Unit Minute = new Unit("minute", "min", 60.0 * Second);
        public static readonly Unit Hour = new Unit("hour", "h", 3600.0 * Second);
        public static readonly Unit Day = new Unit("day", "d", 24.0 * Hour);
    }
}