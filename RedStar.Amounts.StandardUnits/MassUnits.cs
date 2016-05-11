namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class MassUnits
    {
        public static readonly Unit KiloGram = new Unit("kilogram", "Kg", SIUnitTypes.Mass);
        public static readonly Unit Gram = new Unit("gram", "g", 0.001 * KiloGram);
        public static readonly Unit MilliGram = new Unit("milligram", "mg", 0.001 * Gram);
        public static readonly Unit Ton = new Unit("ton", "ton", 1000.0 * KiloGram);
    }
}