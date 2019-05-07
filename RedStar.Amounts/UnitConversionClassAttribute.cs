using System;

namespace RedStar.Amounts
{
    /// <summary>
    /// Attribute to mark classes having static methods that register
    /// conversion functions. The UnitConvert class uses this attribute to
    /// identify classes with unit conversion methods in its RegisterConversions
    /// method.
    /// </summary>
    /// <see cref="UnitManager.RegisterConversions(System.Reflection.Assembly)"/>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UnitConversionClassAttribute : Attribute
    {
    }
}