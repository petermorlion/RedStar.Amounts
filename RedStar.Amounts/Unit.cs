using System;

namespace RedStar.Amounts
{
    [Serializable]
    public sealed class Unit : IComparable, IComparable<Unit>, IEquatable<Unit>, IFormattable
    {
        private static Unit none = new Unit(String.Empty, String.Empty, UnitType.None);

        private string name;
        private string symbol;
        private double factor;
        private UnitType unitType;
        private bool isNamed;

        #region Constructor methods

        public Unit(string name, string symbol, UnitType unitType)
            : this(name, symbol, 1.0, unitType, true)
        {
        }

        public Unit(string name, string symbol, Unit baseUnit)
            : this(name, symbol, baseUnit.factor, baseUnit.unitType, true)
        {
        }

        private Unit(string name, string symbol, double factor, UnitType unitType, bool isNamed)
        {
            this.name = name;
            this.symbol = symbol;
            this.factor = factor;
            this.unitType = unitType;
            this.isNamed = isNamed;

            this.symbol = SanitizeUnitString(this.symbol);
        }

        /// <summary>
        /// None unit.
        /// </summary>
        public static Unit None
        {
            get { return Unit.none; }
        }

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
            get { return this.name; }
        }

        /// <summary>
        /// Gets the symbol of the unit.
        /// </summary>
        public string Symbol
        {
            get { return this.symbol; }
        }

        /// <summary>
        /// Gets the factor of the unit.
        /// </summary>
        public double Factor
        {
            get { return this.factor; }
        }

        /// <summary>
        /// Whether the unit is named.
        /// </summary>
        public bool IsNamed
        {
            get { return this.isNamed; }
        }

        /// <summary>
        /// Gets the type of the unit.
        /// </summary>
        public UnitType UnitType
        {
            get { return this.unitType; }
        }

        /// <summary>
        /// Checks whether the given unit is compatible to this one.
        /// Raises an exception if not compatible.
        /// </summary>
        /// <exception cref="UnitConversionException">Raised when units are not compatible.</exception>
        public void AssertCompatibility(Unit compatibleUnit)
        {
            if (!this.IsCompatibleTo(compatibleUnit)) throw new UnitConversionException(this, compatibleUnit);
        }

        /// <summary>
        /// Checks whether the passed unit is compatible with this one.
        /// </summary>
        public bool IsCompatibleTo(Unit otherUnit)
        {
            return (this.unitType == (otherUnit ?? Unit.none).unitType);
        }

        /// <summary>
        /// Returns a unit by raising the present unit to the specified power.
        /// I.e. meter.Power(3) would return a cubic meter unit.
        /// </summary>
        public Unit Power(int power)
        {
            return new Unit(String.Concat('(', this.name, '^', power, ')'), this.symbol + '^' + power, (double)Math.Pow((double)this.factor, (double)power), this.unitType.Power(power), false);
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
            return this.factor.GetHashCode() ^ this.unitType.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        public override string ToString()
        {
            return this.ToString(null, null);
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        public string ToString(string format)
        {
            return this.ToString(format, null);
        }

        /// <summary>
        /// Returns a string representation of the unit.
        /// </summary>
        public string ToString(IFormatProvider formatProvider)
        {
            return this.ToString(null, formatProvider);
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
                ICustomFormatter formatter = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, formatProvider);
                }
            }

            switch (format)
            {
                case "UN":
                    return this.Name;
                case "US":
                default:
                    return this.Symbol;
            }
        }

        #endregion Public implementation

        /// <summary>
        /// Removes double * and / characters. These could be added after parsing.
        /// </summary>
        internal static string SanitizeUnitString(string s)
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

            return (left.factor == right.factor) && (left.unitType == right.unitType);
        }

        public static bool operator !=(Unit left, Unit right)
        {
            return !(left == right);
        }

        public static Unit operator *(Unit left, Unit right)
        {
            left = left ?? Unit.none;
            right = right ?? Unit.none;
            return new Unit(String.Concat('(', left.name, '*', right.name, ')'), left.symbol + '*' + right.symbol, left.factor * right.factor, left.unitType * right.unitType, false);
        }

        public static Unit operator *(Unit left, double right)
        {
            return (right * left);
        }

        public static Unit operator *(double left, Unit right)
        {
            right = right ?? Unit.none;
            return new Unit(String.Concat('(', left.ToString(), '*', right.name, ')'), left.ToString() + '*' + right.symbol, left * right.factor, right.unitType, false);
        }

        public static Unit operator /(Unit left, Unit right)
        {
            left = left ?? Unit.none;
            right = right ?? Unit.none;
            return new Unit(String.Concat('(', left.name, '/', right.name, ')'), left.symbol + '/' + right.symbol, left.factor / right.factor, left.unitType / right.unitType, false);
        }

        public static Unit operator /(double left, Unit right)
        {
            right = right ?? Unit.none;
            return new Unit(String.Concat('(', left.ToString(), '*', right.name, ')'), left.ToString() + '*' + right.symbol, left / right.factor, right.unitType.Power(-1), false);
        }

        public static Unit operator /(Unit left, double right)
        {
            left = left ?? Unit.none;
            return new Unit(String.Concat('(', left.name, '/', right.ToString(), ')'), left.symbol + '/' + right.ToString(), left.factor / right, left.unitType, false);
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
            this.AssertCompatibility(other);
            if (this.factor < other.factor) return -1;
            else if (this.factor > other.factor) return +1;
            else return 0;
        }

        #endregion IComparable implementation
    }
}
