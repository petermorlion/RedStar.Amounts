namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class ForceUnits
    {
        public static readonly Unit Newton = new Unit("newton", "N", LengthUnits.Meter * MassUnits.KiloGram * TimeUnits.Second.Power(-2));
        public static readonly Unit MicroNewton = new Unit("micronewton", "µN", 0.000001 * Newton);

        public static readonly Unit Dyne = new Unit("dyne", "dyn", 0.00001 * Newton);
    }
}