using System;
#if NET46
using System.Runtime.Serialization;
#endif

namespace RedStar.Amounts
{
    /// <summary>
    /// Exception thrown whenever an exception is referenced by name, but no
    /// unit with the given name is known (registered to the UnitManager).
    /// </summary>
#if NET46
    [Serializable]
#endif
    public class UnknownUnitException
#if NET46
        : ApplicationException
#else
        : Exception
#endif
    {

        public UnknownUnitException() : base() { }

        public UnknownUnitException(string message)
#if NET46
            : base(message)
#endif
        { }

#if NET46
        protected UnknownUnitException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
#endif
    }
}