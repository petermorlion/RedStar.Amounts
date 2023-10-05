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

        /// <summary>Creates a new Amount in Meters.</summary>
        /// <param name="value">The value in Meters</param>
        /// <returns>A new Amount in Meters</returns>
        public static Amount Meters(this double value)
        {
            return new Amount(value, LengthUnits.Meter);
        }

        /// <summary>Creates a new Amount in PicoMeters.</summary>
        /// <param name="value">The value in PicoMeters</param>
        /// <returns>A new Amount in PicoMeters</returns>
        public static Amount PicoMeters(this double value)
        {
            return new Amount(value, LengthUnits.PicoMeter);
        }

        /// <summary>Creates a new Amount in NanoMeters.</summary>
        /// <param name="value">The value in NanoMeters</param>
        /// <returns>A new Amount in NanoMeters</returns>
        public static Amount NanoMeters(this double value)
        {
            return new Amount(value, LengthUnits.NanoMeter);
        }

        /// <summary>Creates a new Amount in MicroMeters.</summary>
        /// <param name="value">The value in MicroMeters</param>
        /// <returns>A new Amount in MicroMeters</returns>
        public static Amount MicroMeters(this double value)
        {
            return new Amount(value, LengthUnits.MicroMeter);
        }

        /// <summary>Creates a new Amount in MilliMeters.</summary>
        /// <param name="value">The value in MilliMeters</param>
        /// <returns>A new Amount in MilliMeters</returns>
        public static Amount MilliMeters(this double value)
        {
            return new Amount(value, LengthUnits.MilliMeter);
        }

        /// <summary>Creates a new Amount in CentiMeters.</summary>
        /// <param name="value">The value in CentiMeters</param>
        /// <returns>A new Amount in CentiMeters</returns>
        public static Amount CentiMeters(this double value)
        {
            return new Amount(value, LengthUnits.CentiMeter);
        }

        /// <summary>Creates a new Amount in DeciMeters.</summary>
        /// <param name="value">The value in DeciMeters</param>
        /// <returns>A new Amount in DeciMeters</returns>
        public static Amount DeciMeters(this double value)
        {
            return new Amount(value, LengthUnits.DeciMeter);
        }

        /// <summary>Creates a new Amount in DecaMeters.</summary>
        /// <param name="value">The value in DecaMeters</param>
        /// <returns>A new Amount in DecaMeters</returns>
        public static Amount DecaMeters(this double value)
        {
            return new Amount(value, LengthUnits.DecaMeter);
        }

        /// <summary>Creates a new Amount in HectoMeters.</summary>
        /// <param name="value">The value in HectoMeters</param>
        /// <returns>A new Amount in HectoMeters</returns>
        public static Amount HectoMeters(this double value)
        {
            return new Amount(value, LengthUnits.HectoMeter);
        }

        /// <summary>Creates a new Amount in KiloMeters.</summary>
        /// <param name="value">The value in KiloMeters</param>
        /// <returns>A new Amount in KiloMeters</returns>
        public static Amount KiloMeters(this double value)
        {
            return new Amount(value, LengthUnits.KiloMeter);
        }

        /// <summary>Creates a new Amount in Inches.</summary>
        /// <param name="value">The value in Inches</param>
        /// <returns>A new Amount in Inches</returns>
        public static Amount Inches(this double value)
        {
            return new Amount(value, LengthUnits.Inch);
        }

        /// <summary>Creates a new Amount in Feet.</summary>
        /// <param name="value">The value in Feet</param>
        /// <returns>A new Amount in Feet</returns>
        public static Amount Feet(this double value)
        {
            return new Amount(value, LengthUnits.Foot);
        }

        /// <summary>Creates a new Amount in Yards.</summary>
        /// <param name="value">The value in Yards</param>
        /// <returns>A new Amount in Yards</returns>
        public static Amount Yards(this double value)
        {
            return new Amount(value, LengthUnits.Yard);
        }

        /// <summary>Creates a new Amount in Miles.</summary>
        /// <param name="value">The value in Miles</param>
        /// <returns>A new Amount in Miles</returns>
        public static Amount Miles(this double value)
        {
            return new Amount(value, LengthUnits.Mile);
        }

        /// <summary>Creates a new Amount in NauticalMiles.</summary>
        /// <param name="value">The value in NauticalMiles</param>
        /// <returns>A new Amount in NauticalMiles</returns>
        public static Amount NauticalMiles(this double value)
        {
            return new Amount(value, LengthUnits.NauticalMile);
        }

        /// <summary>Creates a new Amount in LightYears.</summary>
        /// <param name="value">The value in LightYears</param>
        /// <returns>A new Amount in LightYears</returns>
        public static Amount LightYears(this double value)
        {
            return new Amount(value, LengthUnits.LightYear);
        }
    }
}