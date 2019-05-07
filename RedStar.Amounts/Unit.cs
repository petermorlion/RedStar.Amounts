using System;

namespace RedStar.Amounts
{
    [Serializable]
    public sealed class Unit : IComparable, IComparable<Unit>, IEquatable<Unit>, IFormattable
    {
        private static readonly Unit _none = new Unit(string.Empty, string.Empty, UnitType.None);

        private readonly string _name;
        private readonly string _symbol;
        private readonly double _factor;
        private readonly UnitType _unitType;
        private readonly bool _isNamed;

        #region Constructor methods

        public Unit(string name, string symbol, UnitType unitType)
            : this(name, symbol, 1.0, unitType, true)
        {
        }

        public Unit(string name, string symbol, Unit baseUnit)
            : this(name, symbol, baseUnit._factor, baseUnit._unitType, true)
        {
        }

        private Unit(string name, string symbol, double factor, UnitType unitType, bool isNamed)
        {
            _name = name;
            _symbol = symbol;
            _factor = factor;
            _unitType = unitType;
            _isNamed = isNamed;

            _symbol = SanitizeUnitString(_symbol);
        }

        /// <summary>
        /// None unit.
        /// </summary>
        public static Unit None
        {
            get { return _none; }
        }

        /// <summary>
        /// Converts the string representation of a unit to a Unit object. 
        /// The string representation can be the name or the symbol of the unit.
        /// </summary>
        /// <param name="s">A string containing the name or the symbol of a unit to convert.</param>
        /// <returns>A Unit object equivalent to the provided string.</returns>
        public static Unit Parse(string s)
        {
            return UnitParser.Parse(s);
        }

        #endregion Constructor methods

        #region Public implementation

        /// <summary>
        /// Gets the name of the unit.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the symbol of the unit.
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
        }

        /// <summary>
        /// Gets the factor of the unit.
        /// </summary>
        public double Factor
        {
            get { return _factor; }
        }

        /// <summary>
        /// Whether the unit is named.
        /// </summary>
        public bool IsNamed
        {
            get { return _isNamed; }
        }

        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        public UnitType UnitType
        {
            get { return _unitType; }
        }

        /// <summary>
        /// Checks whether the given unit is compatible to this one.
        /// Raises an exception if not compatible.
        /// </summary>
        /// <exception cref="UnitConversionException">Raised when units are not compatible.</exception>
        public void AssertCompatibility(Unit compatibleUnit)
        {
            if (!IsCompatibleTo(compatibleUnit)) throw new UnitConversionException(this, compatibleUnit);
        }

        /// <summary>
        /// Checks whether the passed unit is compatible with this one.
        /// </summary>
        public bool IsCompatibleTo(Unit otherUnit)
        {
            return (_unitType == (otherUnit ?? _none)._unitType);
        }

        /// <summary>
        /// Returns a unit by raising the present unit to the specified power.
        /// I.e. meter.Power(3) would return a cubic meter unit.
        /// </summary>
        public Unit Power(int power)
        {
            return new Unit(String.Concat('(', _name, '^', power, ')'), _symbol + '^' + power, Math.Pow(_factor, power), _unitType.Power(power), false);
        }

        /// <summary>
        /// Tests equality of both objects.
        /// </summary>
        public override bool Equals(object obj)
        {
            return (this == (obj as Unit));
        }

        /// <summary>
        /// Tests equality of both objects.
        /// </summary>
        public bool Equals(Unit unit)
        {
            return (this == unit);
        }

        /// <summary>
        /// Returns the hashcode of this unit.
        /// </summary>
        public override int GetHashCode()
        {
            return _factor.GetHashCode() ^ _unitType.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        /// <remarks>
        /// The format string can be either 'UN' (Unit Name) or 'US' (Unit Symbol).
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null) format = "US";

            if (formatProvider != null)
            {
                ICustomFormatter formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, formatProvider);
                }
            }

            switch (format)
            {
                case "UN":
                    return Name;
                case "US":
                default:
                    return Symbol;
            }
        }

        #endregion Public implementation

        /// <summary>
        /// Removes double * and / characters. These could be added after parsing.
        /// </summary>
        private static string SanitizeUnitString(string s)
        {
            while (s.Contains("**"))
            {
                s = s.Replace("**", "*");
            }

            while (s.Contains("//"))
            {
                s = s.Replace("//", "/");
            }

            return s;
        }

        #region Operator overloads

        public static bool operator ==(Unit left, Unit right)
        {
            // Special cases:
            if (ReferenceEquals(left, right))
                return true;

            // Compare content:
            // Compare content:
            if ((object)left == null)
                return false;

            if ((object)right == null)
                return false;

            return (left._factor == right._factor) && (left._unitType == right._unitType);
        }

        public static bool operator !=(Unit left, Unit right)
        {
            return !(left == right);
        }

        public static Unit operator *(Unit left, Unit right)
        {
            left = left ?? _none;
            right = right ?? _none;
            return new Unit(string.Concat('(', left._name, '*', right._name, ')'), left._symbol + '*' + right._symbol, left._factor * right._factor, left._unitType * right._unitType, false);
        }

        public static Unit operator *(Unit left, double right)
        {
            return (right * left);
        }

        public static Unit operator *(double left, Unit right)
        {
            if (left == 1)
            {
                return right;
            }

            right = right ?? _none;
            return new Unit(string.Concat('(', left.ToString(), '*', right._name, ')'), left.ToString() + '*' + right._symbol, left * right._factor, right._unitType, false);
        }

        public static Unit operator /(Unit left, Unit right)
        {
            left = left ?? _none;
            right = right ?? _none;
            return new Unit(string.Concat('(', left._name, '/', right._name, ')'), left._symbol + '/' + right._symbol, left._factor / right._factor, left._unitType / right._unitType, false);
        }

        public static Unit operator /(double left, Unit right)
        {
            right = right ?? _none;
            return new Unit(string.Concat('(', left.ToString(), '*', right._name, ')'), left.ToString() + '*' + right._symbol, left / right._factor, right._unitType.Power(-1), false);
        }

        public static Unit operator /(Unit left, double right)
        {
            left = left ?? _none;
            return new Unit(string.Concat('(', left._name, '/', right.ToString(), ')'), left._symbol + '/' + right.ToString(), left._factor / right, left._unitType, false);
        }

        #endregion Operator overloads

        #region IComparable implementation

        /// <summary>
        /// Compares the passed unit to the current one. Allows sorting units of the same type.
        /// </summary>
        /// <remarks>Only compatible units can be compared.</remarks>
        int IComparable.CompareTo(object obj)
        {
            return ((IComparable<Unit>)this).CompareTo((Unit)obj);
        }

        /// <summary>
        /// Compares the passed unit to the current one. Allows sorting units of the same type.
        /// </summary>
        /// <remarks>Only compatible units can be compared.</remarks>
        int IComparable<Unit>.CompareTo(Unit other)
        {
            AssertCompatibility(other);
            if (_factor < other._factor) return -1;
            else if (_factor > other._factor) return +1;
            else return 0;
        }

        #endregion IComparable implementation
    }
}
