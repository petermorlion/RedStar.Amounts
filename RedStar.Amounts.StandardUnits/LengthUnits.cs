namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class LengthUnits
    {
        public static readonly Unit Meter = new Unit("meter", "m", SIUnitTypes.Length);
        public static readonly Unit PicoMeter = new Unit("picometer", "pm", 0.000000000001 * Meter);
        public static readonly Unit NanoMeter = new Unit("nanometer", "nm", 0.000000001 * Meter);
        public static readonly Unit MicroMeter = new Unit("micrometer", "µm", 0.000001 * Meter);
        public static readonly Unit MilliMeter = new Unit("millimeter", "mm", 0.001 * Meter);
        public static readonly Unit CentiMeter = new Unit("centimeter", "cm", 0.01 * Meter);
        public static readonly Unit DeciMeter = new Unit("decimeter", "dm", 0.1 * Meter);
        public static readonly Unit DecaMeter = new Unit("decameter", "Dm", 10.0 * Meter);
        public static readonly Unit HectoMeter = new Unit("hectometer", "Hm", 100.0 * Meter);
        public static readonly Unit KiloMeter = new Unit("kilometer", "km", 1000.0 * Meter);

        public static readonly Unit Inch = new Unit("inch", "in", 0.0254 * Meter);
        public static readonly Unit Foot = new Unit("foot", "ft", 12.0 * Inch);
        public static readonly Unit Yard = new Unit("yard", "yd", 36.0 * Inch);
        public static readonly Unit Mile = new Unit("mile", "mi", 5280.0 * Foot);
        public static readonly Unit NauticalMile = new Unit("nautical mile", "nmi", 1852.0 * Meter);

        public static readonly Unit LightYear = new Unit("light-year", "ly", 9460730472580800.0 * Meter);
    }
}