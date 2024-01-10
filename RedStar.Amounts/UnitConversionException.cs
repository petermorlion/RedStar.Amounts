using System;

namespace RedStar.Amounts
{
    /// <summary>
    /// Exception thrown when a unit conversion failed, i.e. because you are converting
    /// amounts from one unit into another non-compatible unit.
    /// </summary>
    public class UnitConversionException : InvalidOperationException
    {
        public UnitConversionException() : base() { }

        public UnitConversionException(string message) : base(message) { }

        public UnitConversionException(Unit fromUnit, Unit toUnit) : this(String.Format("Failed to convert from unit '{0}' to unit '{1}'. Units are not compatible and no conversions are defined.", fromUnit.Name, toUnit.Name)) { }
    }
}