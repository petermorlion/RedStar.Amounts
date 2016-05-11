using System;

namespace RedStar.Amounts
{
    [Serializable]
    public sealed class Amount :
        ICloneable,
        IComparable,
        IComparable<Amount>,
        IConvertible,
        IEquatable<Amount>,
        IFormattable,
        IUnitConsumer
    {
        private static int equalityPrecision = 8;

        private double value;
        private Unit unit;

        #region Constructor methods

        public Amount(double value, Unit unit)
        {
            this.value = value;
            this.unit = unit;
        }

        public Amount(double value, string unitName)
        {
            this.value = value;
            this.unit = UnitManager.GetUnitByName(unitName);
        }

        public static Amount Zero(Unit unit)
        {
            return new Amount(0.0, unit);
        }

        public static Amount Zero(string unitName)
        {
            return new Amount(0.0, unitName);
        }

        #endregion Constructor methods

        #region Public implementation

        /// <summary>
        /// The precision to which two amounts are considered equal.
        /// </summary>
        public static int EqualityPrecision
        {
            get { return Amount.equalityPrecision; }
            set { Amount.equalityPrecision = value; }
        }

        /// <summary>
        /// Gets the raw value of the amount.
        /// </summary>
        public double Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the unit of the amount.
        /// </summary>
        public Unit Unit
        {
            get { return this.unit; }
        }

        /// <summary>
        /// Returns a unit that matches this amount.
        /// </summary>
        public Unit AsUnit()
        {
            return new Unit(this.value + "*" + this.unit.Name, this.value + "*" + this.unit.Symbol, this.Value * this.Unit);
        }

        /// <summary>
        /// Returns a clone of the Amount object.
        /// </summary>
        public object Clone()
        {
            // Actually, as Amount is immutable, it can safely return itself:
            return this;
        }

        /// <summary>
        /// Returns a matching amount converted to the given unit and rounded
        /// up to the given number of decimals.
        /// </summary>
        public Amount ConvertedTo(string unitName, int decimals)
        {
            return this.ConvertedTo(UnitManager.GetUnitByName(unitName), decimals);
        }

        /// <summary>
        /// Returns a matching amount converted to the given unit and rounded
        /// up to the given number of decimals.
        /// </summary>
        public Amount ConvertedTo(Unit unit, int decimals)
        {
            return new Amount(Math.Round(UnitManager.ConvertTo(this, unit).Value, decimals), unit);
        }

        /// <summary>
        /// Returns a matching amount converted to the given unit.
        /// </summary>
        public Amount ConvertedTo(string unitName)
        {
            return this.ConvertedTo(UnitManager.GetUnitByName(unitName));
        }

        /// <summary>
        /// Returns a matching amount converted to the given unit.
        /// </summary>
        public Amount ConvertedTo(Unit unit)
        {
            // Let UnitManager perform conversion:
            return UnitManager.ConvertTo(this, unit);
        }

        /// <summary>
        /// Splits this amount into integral values of the given units
        /// except for the last amount which is rounded up to the number
        /// of decimals given.
        /// </summary>
        public Amount[] Split(Unit[] units, int decimals)
        {
            Amount[] amounts = new Amount[units.Length];
            Amount rest = this;

            // Truncate for all but the last unit:
            for (int i = 0; i < (units.Length - 1); i++)
            {
                amounts[i] = (Amount)rest.ConvertedTo(units[i]).MemberwiseClone();
                amounts[i].value = Math.Truncate(amounts[i].value);
                rest = rest - amounts[i];
            }

            // Handle the last unit:
            amounts[units.Length - 1] = rest.ConvertedTo(units[units.Length - 1], decimals);

            return amounts;
        }

        public override bool Equals(object obj)
        {
            return (this == (obj as Amount));
        }

        public bool Equals(Amount amount)
        {
            return (this == amount);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode() ^ this.unit.GetHashCode();
        }

        /// <summary>
        /// Shows the default string representation of the amount. (The default format string is "GG").
        /// </summary>
        public override string ToString()
        {
            return this.ToString((string)null, (IFormatProvider)null);
        }

        /// <summary>
        /// Shows a string representation of the amount, formatted according to the passed format string.
        /// </summary>
        public string ToString(string format)
        {
            return this.ToString(format, (IFormatProvider)null);
        }

        /// <summary>
        /// Shows the default string representation of the amount using the given format provider.
        /// </summary>
        public string ToString(IFormatProvider formatProvider)
        {
            return this.ToString((string)null, formatProvider);
        }

        /// <summary>
        /// Shows a string representation of the amount, formatted according to the passed format string,
        /// using the given format provider.
        /// </summary>
        /// <remarks>
        /// Valid format strings are 'GG', 'GN', 'GS', 'NG', 'NN', 'NS' (where the first letter represents
        /// the value formatting (General, Numeric), and the second letter represents the unit formatting
        /// (General, Name, Symbol)), or a custom number format with 'UG', 'UN' or 'US' (UnitGeneral,
        /// UnitName or UnitSymbol) representing the unit (i.e. "#,##0.00 UL"). The format string can also
        /// contains a '|' followed by a unit to convert to.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null) format = "GG";

            if (formatProvider != null)
            {
                ICustomFormatter formatter = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, formatProvider);
                }
            }

            String[] formats = format.Split('|');
            Amount amount = this;
            if (formats.Length >= 2)
            {
                if (formats[1] == "?")
                    amount = amount.ConvertedTo(UnitManager.ResolveToNamedUnit(amount.Unit, true));
                else
                    amount = amount.ConvertedTo(formats[1]);
            }

            switch (formats[0])
            {
                case "GG":
                    return String.Format(formatProvider, "{0:G} {1}", amount.Value, amount.Unit).TrimEnd(null);
                case "GN":
                    return String.Format(formatProvider, "{0:G} {1:UN}", amount.Value, amount.Unit).TrimEnd(null);
                case "GS":
                    return String.Format(formatProvider, "{0:G} {1:US}", amount.Value, amount.Unit).TrimEnd(null);
                case "NG":
                    return String.Format(formatProvider, "{0:N} {1}", amount.Value, amount.Unit).TrimEnd(null);
                case "NN":
                    return String.Format(formatProvider, "{0:N} {1:UN}", amount.Value, amount.Unit).TrimEnd(null);
                case "NS":
                    return String.Format(formatProvider, "{0:N} {1:US}", amount.Value, amount.Unit).TrimEnd(null);
                default:
                    formats[0] = formats[0].Replace("UG", "\"" + amount.Unit.ToString("", formatProvider) + "\"");
                    formats[0] = formats[0].Replace("UN", "\"" + amount.Unit.ToString("UN", formatProvider) + "\"");
                    formats[0] = formats[0].Replace("US", "\"" + amount.Unit.ToString("US", formatProvider) + "\"");
                    return amount.Value.ToString(formats[0], formatProvider).TrimEnd(null);
            }
        }

        /// <summary>
        /// Static convenience ToString method, returns ToString of the amount,
        /// or empty string if amount is null.
        /// </summary>
        public static string ToString(Amount amount)
        {
            return ToString(amount, (string)null, (IFormatProvider)null);
        }

        /// <summary>
        /// Static convenience ToString method, returns ToString of the amount,
        /// or empty string if amount is null.
        /// </summary>
        public static string ToString(Amount amount, string format)
        {
            return ToString(amount, format, (IFormatProvider)null);
        }

        /// <summary>
        /// Static convenience ToString method, returns ToString of the amount,
        /// or empty string if amount is null.
        /// </summary>
        public static string ToString(Amount amount, IFormatProvider formatProvider)
        {
            return ToString(amount, (string)null, formatProvider);
        }

        /// <summary>
        /// Static convenience ToString method, returns ToString of the amount,
        /// or empty string if amount is null.
        /// </summary>
        public static string ToString(Amount amount, string format, IFormatProvider formatProvider)
        {
            if (amount == null) return String.Empty;
            else return amount.ToString(format, formatProvider);
        }

        #endregion Public implementation

        #region Mathematical operations

        /// <summary>
        /// Adds this with the amount (= this + amount).
        /// </summary>
        public Amount Add(Amount amount)
        {
            return (this + amount);
        }

        /// <summary>
        /// Negates this (= -this).
        /// </summary>
        public Amount Negate()
        {
            return (-this);
        }

        /// <summary>
        /// Multiply this with amount (= this * amount).
        /// </summary>
        public Amount Multiply(Amount amount)
        {
            return (this * amount);
        }

        /// <summary>
        /// Multiply this with value (= this * value).
        /// </summary>
        public Amount Multiply(double value)
        {
            return (this * value);
        }

        /// <summary>
        /// Divides this by amount (= this / amount).
        /// </summary>
        public Amount DivideBy(Amount amount)
        {
            return (this / amount);
        }

        /// <summary>
        /// Divides this by value (= this / value).
        /// </summary>
        public Amount DivideBy(double value)
        {
            return (this / value);
        }

        /// <summary>
        /// Returns 1 over this amount (= 1 / this).
        /// </summary>
        public Amount Inverse()
        {
            return (1.0 / this);
        }

        /// <summary>
        /// Raises this amount to the given power.
        /// </summary>
        public Amount Power(int power)
        {
            return new Amount(Math.Pow(this.value, power), this.unit.Power(power));
        }

        #endregion Mathematical operations

        #region Operator overloads

        /// <summary>
        /// Compares two amounts.
        /// </summary>
        public static bool operator ==(Amount left, Amount right)
        {
            // Check references:
            if (Object.ReferenceEquals(left, right))
                return true;
            else if (Object.ReferenceEquals(left, null))
                return false;
            else if (Object.ReferenceEquals(right, null))
                return false;

            // Check value:
            try
            {
                return Math.Round(left.value, Amount.equalityPrecision)
                    == Math.Round(right.ConvertedTo(left.Unit).value, Amount.equalityPrecision);
            }
            catch (UnitConversionException)
            {
                return false;
            }
        }

        /// <summary>
        /// Compares two amounts.
        /// </summary>
        public static bool operator !=(Amount left, Amount right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Compares two amounts of compatible units.
        /// </summary>
        public static bool operator <(Amount left, Amount right)
        {
            Amount rightConverted = right.ConvertedTo(left.unit);
            return (left != rightConverted) && (left.value < rightConverted.value);
        }

        /// <summary>
        /// Compares two amounts of compatible units.
        /// </summary>
        public static bool operator <=(Amount left, Amount right)
        {
            Amount rightConverted = right.ConvertedTo(left.unit);
            return (left == rightConverted) || (left.value < rightConverted.value);
        }

        /// <summary>
        /// Compares two amounts of compatible units.
        /// </summary>
        public static bool operator >(Amount left, Amount right)
        {
            Amount rightConverted = right.ConvertedTo(left.unit);
            return (left != rightConverted) && (left.value > rightConverted.value);
        }

        /// <summary>
        /// Compares two amounts of compatible units.
        /// </summary>
        public static bool operator >=(Amount left, Amount right)
        {
            Amount rightConverted = right.ConvertedTo(left.unit);
            return (left == rightConverted) || (left.value > rightConverted.value);
        }

        /// <summary>
        /// Unary '+' operator.
        /// </summary>
        public static Amount operator +(Amount right)
        {
            return right;
        }

        /// <summary>
        /// Additions two amounts of compatible units.
        /// </summary>
        public static Amount operator +(Amount left, Amount right)
        {
            if ((left == null) && (right == null)) return null;
            left = left ?? Zero((right != null) ? right.unit : Unit.None);
            right = right ?? Zero(left.Unit);
            return new Amount(left.value + right.ConvertedTo(left.unit).value, left.unit);
        }

        /// <summary>
        /// Unary '-' operator.
        /// </summary>
        public static Amount operator -(Amount right)
        {
            if (Object.ReferenceEquals(right, null))
                return null;
            else
                return new Amount(-right.value, right.unit);
        }

        /// <summary>
        /// Substracts two amounts of compatible units.
        /// </summary>
        public static Amount operator -(Amount left, Amount right)
        {
            return (left + (-right));
        }

        /// <summary>
        /// Multiplies two amounts.
        /// </summary>
        public static Amount operator *(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, null))
                return null;
            else if (Object.ReferenceEquals(right, null))
                return null;
            else
                return new Amount(left.value * right.value, left.unit * right.unit);
        }

        /// <summary>
        /// Divides two amounts.
        /// </summary>
        public static Amount operator /(Amount left, Amount right)
        {
            if (Object.ReferenceEquals(left, null))
                return null;
            else if (Object.ReferenceEquals(right, null))
                return null;
            else
                return new Amount(left.value / right.value, left.unit / right.unit);
        }

        /// <summary>
        /// Multiplies an amount with a double value.
        /// </summary>
        public static Amount operator *(Amount left, double right)
        {
            if (Object.ReferenceEquals(left, null))
                return null;
            else
                return new Amount(left.value * right, left.unit);
        }

        /// <summary>
        /// Divides an amount by a double value.
        /// </summary>
        public static Amount operator /(Amount left, double right)
        {
            if (Object.ReferenceEquals(left, null))
                return null;
            else
                return new Amount(left.value / right, left.unit);
        }

        /// <summary>
        /// Multiplies a double value with an amount.
        /// </summary>
        public static Amount operator *(double left, Amount right)
        {
            if (Object.ReferenceEquals(right, null))
                return null;
            else
                return new Amount(left * right.value, right.unit);
        }

        /// <summary>
        /// Divides a double value by an amount.
        /// </summary>
        public static Amount operator /(double left, Amount right)
        {
            if (Object.ReferenceEquals(right, null))
                return null;
            else
                return new Amount(left / right.value, 1.0 / right.unit);
        }

        /// <summary>
        /// Casts a double value to an amount expressed in the None unit.
        /// </summary>
        public static explicit operator Amount(double value)
        {
            return new Amount(value, Unit.None);
        }

        /// <summary>
        /// Casts an amount expressed in the None unit to a double.
        /// </summary>
        public static explicit operator double?(Amount amount)
        {
            try
            {
                if (amount == null) return null;
                else return amount.ConvertedTo(Unit.None).Value;
            }
            catch (UnitConversionException)
            {
                throw new InvalidCastException("An amount can only be casted to a numeric type if it is expressed in a None unit.");
            }
        }

        #endregion Operator overloads

        #region IConvertible implementation

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to boolean.");
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to byte.");
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to char.");
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to DateTime.");
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)(double)this;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)this;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (Int16)((double)this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (Int32)((double)this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (Int64)((double)this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to signed byte.");
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)((double)this);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString(provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(Double))
            {
                return Convert.ToDouble(this);
            }
            else if (conversionType == typeof(Single))
            {
                return Convert.ToSingle(this);
            }
            if (conversionType == typeof(Decimal))
            {
                return Convert.ToDecimal(this);
            }
            else if (conversionType == typeof(Int16))
            {
                return Convert.ToInt16(this);
            }
            else if (conversionType == typeof(Int32))
            {
                return Convert.ToInt32(this);
            }
            else if (conversionType == typeof(Int64))
            {
                return Convert.ToInt64(this);
            }
            else if (conversionType == typeof(String))
            {
                return Convert.ToString(this, provider);
            }
            else
            {
                throw new InvalidCastException(String.Format("An Amount cannot be converted to the requested type {0}.", conversionType));
            }
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to unsigned Int16.");
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to unsigned Int32.");
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("An Amount cannot be converted to unsigned Int64.");
        }

        #endregion IConvertible implementation

        #region IComparable implementation

        /// <summary>
        /// Compares two amounts of compatible units.
        /// </summary>
        int IComparable.CompareTo(object obj)
        {
            Amount other = obj as Amount;
            if (other == null) return +1;
            return ((IComparable<Amount>)this).CompareTo(other);
        }

        /// <summary>
        /// Compares two amounts of compatible units.
        /// </summary>
        int IComparable<Amount>.CompareTo(Amount other)
        {
            if (this < other) return -1;
            else if (this > other) return +1;
            else return 0;
        }

        #endregion IComparable implementation
    }
}
