using System;

namespace RedStar.Amounts
{
    /// <summary>
    /// Attribute to mark classes having static unit fields to be registered
    /// by the UnitManager's RegisterUnits method.
    /// </summary>
    /// <see cref="UnitManager.RegisterUnits(System.Reflection.Assembly)"/>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class UnitDefinitionClassAttribute : Attribute
    {
    }
}