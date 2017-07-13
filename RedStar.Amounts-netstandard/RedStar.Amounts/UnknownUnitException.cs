using System;

namespace RedStar.Amounts
{
    /// <summary>
    /// Exception thrown whenever an exception is referenced by name, but no
    /// unit with the given name is known (registered to the UnitManager).
    /// </summary>
    public class UnknownUnitException
        : Exception
    {

        public UnknownUnitException() : base() { }

        public UnknownUnitException(string message)
        { }
    }
}