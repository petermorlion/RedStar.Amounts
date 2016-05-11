namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class RelativeUnits
    {
        public static readonly Unit Absolute = new Unit("absolute", "-", Unit.None);
        public static readonly Unit Percentage = new Unit("percentage", "%", 0.01 * Unit.None);
    }
}