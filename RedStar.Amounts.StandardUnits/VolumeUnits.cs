namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class VolumeUnits
    {
        public static readonly Unit Liter = new Unit("liter", "L", LengthUnits.DeciMeter.Power(3));
        public static readonly Unit MilliLiter = new Unit("milliliter", "mL", 0.001 * Liter);
        public static readonly Unit CentiLiter = new Unit("centiliter", "cL", 0.01 * Liter);
        public static readonly Unit DeciLiter = new Unit("deciliter", "dL", 0.1 * Liter);
        public static readonly Unit HectoLiter = new Unit("hectoliter", "hL", 100.0 * Liter);

        public static readonly Unit Meter3 = new Unit("meter³", "m³", LengthUnits.Meter.Power(3));
    }
}