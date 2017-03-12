namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class FrequencyUnits
    {
        public static readonly Unit Hertz = new Unit("Hertz", "hz", TimeUnits.Second.Power(-1));
        public static readonly Unit MegaHerts = new Unit("MegaHertz", "Mhz", 1000000.0 * Hertz);
        public static readonly Unit RPM = new Unit("Rounds per minute", "rpm", TimeUnits.Minute.Power(-1));
    }
}