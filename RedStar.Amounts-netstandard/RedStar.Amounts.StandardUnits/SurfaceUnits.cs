namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class SurfaceUnits
    {
        public static readonly Unit Meter2 = new Unit("meter²", "m²", LengthUnits.Meter.Power(2));
        public static readonly Unit Are = new Unit("are", "are", 100.0 * Meter2);
        public static readonly Unit HectAre = new Unit("hectare", "ha", 10000.0 * Meter2);
        public static readonly Unit KiloMeter2 = new Unit("kilometer²", "Km²", LengthUnits.KiloMeter.Power(2));
    }
}