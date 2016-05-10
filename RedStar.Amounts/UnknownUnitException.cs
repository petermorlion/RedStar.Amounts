using System;
using System.Runtime.Serialization;

namespace RedStar.Amounts
{
    /// <summary>
    /// Exception thrown whenever an exception is referenced by name, but no
    /// unit with the given name is known (registered to the UnitManager).
    /// </summary>
    [Serializable]
    public class UnknownUnitException : ApplicationException
    {

        public UnknownUnitException() : base() { }

        public UnknownUnitException(string message) : base(message) { }

        protected UnknownUnitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}