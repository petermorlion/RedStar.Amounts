using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace RedStar.Amounts
{
    [Serializable]
    public sealed class UnitType : ISerializable
    {
        #region BaseUnitType support

        private static ReaderWriterLock baseUnitTypeLock = new ReaderWriterLock();
        private static IList<string> baseUnitTypeNames = new List<string>();

        private static string GetBaseUnitName(int index)
        {
            // Lock baseUnitTypeNames:
            baseUnitTypeLock.AcquireReaderLock(2000);

            try
            {
                return baseUnitTypeNames[index];
            }
            finally
            {
                // Release lock:
                baseUnitTypeLock.ReleaseReaderLock();
            }
        }

        private static int GetBaseUnitIndex(string unitTypeName)
        {
            // Verify unitTypeName does not contain pipe char (which is used in serializations):
            if (unitTypeName.Contains('|'))
                throw new ArgumentException("The name of a UnitType must not contain the '|' (pipe) character.", "unitTypeName");

            // Lock baseUnitTypeNames:
            baseUnitTypeLock.AcquireReaderLock(2000);

            try
            {
                // Retrieve index of unitTypeName:
                int index = baseUnitTypeNames.IndexOf(unitTypeName);

                // If not found, register unitTypeName:
                if (index == -1)
                {
                    baseUnitTypeLock.UpgradeToWriterLock(2000);
                    index = baseUnitTypeNames.Count;
                    baseUnitTypeNames.Add(unitTypeName);
                }

                // Return index:
                return index;
            }
            finally
            {
                // Release lock:
                baseUnitTypeLock.ReleaseLock();
            }
        }

        #endregion BaseUnitType support

        private sbyte[] baseUnitIndices;

        [NonSerialized]
        private int cachedHashCode;

        #region Constructor methods

        private static UnitType none = new UnitType(0);

        public UnitType(string unitTypeName)
        {
            int unitIndex = GetBaseUnitIndex(unitTypeName);
            this.baseUnitIndices = new sbyte[unitIndex + 1];
            this.baseUnitIndices[unitIndex] = 1;
        }

        private UnitType(int indicesLength)
        {
            this.baseUnitIndices = new sbyte[indicesLength];
        }

        private UnitType(sbyte[] baseUnitIndices)
        {
            this.baseUnitIndices = (sbyte[])baseUnitIndices.Clone();
        }

        private UnitType(SerializationInfo info, StreamingContext c)
        {
            // Retrieve data from serialization:
            sbyte[] tstoreexp = info.GetString("exps").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToSByte(x))
                .ToArray();
            int[] tstoreind = info.GetString("names").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => UnitType.GetBaseUnitIndex(x))
                .ToArray();

            // Construct instance:
            if (tstoreexp.Length > 0)
            {
                this.baseUnitIndices = new sbyte[tstoreind.Max() + 1];
                for (int i = 0; i < tstoreexp.Length; i++)
                {
                    this.baseUnitIndices[tstoreind[i]] = tstoreexp[i];
                }
            }
            else
            {
                this.baseUnitIndices = new sbyte[0];
            }
        }

        public static UnitType None
        {
            get { return UnitType.none; }
        }

        #endregion Constructor methods

        #region Public implementation

        /// <summary>
        /// Returns the unit type raised to the specified power.
        /// </summary>
        public UnitType Power(int power)
        {
            UnitType result = new UnitType(this.baseUnitIndices);
            for (int i = 0; i < result.baseUnitIndices.Length; i++)
                result.baseUnitIndices[i] = (sbyte)(result.baseUnitIndices[i] * power);
            return result;
        }

        public override bool Equals(object obj)
        {
            return (this == (obj as UnitType));
        }

        public override int GetHashCode()
        {
            if (this.cachedHashCode == 0)
            {
                int hash = 0;
                for (int i = 0; i < this.baseUnitIndices.Length; i++)
                {
                    int factor = i + i + 1;
                    hash += factor * factor * this.baseUnitIndices[i] * this.baseUnitIndices[i];
                }
                this.cachedHashCode = hash;
            }
            return this.cachedHashCode;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string sep = String.Empty;
            for (int i = 0; i < this.baseUnitIndices.Length; i++)
            {
                if (this.baseUnitIndices[i] != 0)
                {
                    sb.Append(sep);
                    sb.Append(GetBaseUnitName(i));
                    sb.Append('^');
                    sb.Append(this.baseUnitIndices[i]);
                    sep = " * ";
                }
            }
            return sb.ToString();
        }

        #endregion Public implementation

        #region Operator overloads

        public static UnitType operator *(UnitType left, UnitType right)
        {
            UnitType result = new UnitType(Math.Max(left.baseUnitIndices.Length, right.baseUnitIndices.Length));
            left.baseUnitIndices.CopyTo(result.baseUnitIndices, 0);
            for (int i = 0; i < right.baseUnitIndices.Length; i++)
                result.baseUnitIndices[i] += right.baseUnitIndices[i];
            return result;
        }

        public static UnitType operator /(UnitType left, UnitType right)
        {
            UnitType result = new UnitType(Math.Max(left.baseUnitIndices.Length, right.baseUnitIndices.Length));
            left.baseUnitIndices.CopyTo(result.baseUnitIndices, 0);
            for (int i = 0; i < right.baseUnitIndices.Length; i++)
                result.baseUnitIndices[i] -= right.baseUnitIndices[i];
            return result;
        }

        public static bool operator ==(UnitType left, UnitType right)
        {
            // Handle special cases:
            if (Object.ReferenceEquals(left, right))
                return true;
            else if (Object.ReferenceEquals(left, null))
                return false;
            else if (Object.ReferenceEquals(right, null))
                return false;

            // Determine longest and shortest baseUnitUndice arrays:
            sbyte[] longest, shortest;
            int leftlen = left.baseUnitIndices.Length;
            int rightlen = right.baseUnitIndices.Length;
            if (leftlen > rightlen)
            {
                longest = left.baseUnitIndices;
                shortest = right.baseUnitIndices;
            }
            else
            {
                longest = right.baseUnitIndices;
                shortest = left.baseUnitIndices;
            }

            // Compare baseUnitIndice array content:
            for (int i = 0; i < shortest.Length; i++)
                if (shortest[i] != longest[i]) return false;
            for (int i = shortest.Length; i < longest.Length; i++)
                if (longest[i] != 0) return false;
            return true;
        }

        public static bool operator !=(UnitType left, UnitType right)
        {
            return !(left == right);
        }

        #endregion Operator overloads

        #region ISerializable Members

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            bool first = true;
            StringBuilder sbn = new StringBuilder(this.baseUnitIndices.Length * 8);
            StringBuilder sbx = new StringBuilder(this.baseUnitIndices.Length * 4);
            for (int i = 0; i < this.baseUnitIndices.Length; i++)
            {
                if (this.baseUnitIndices[i] != 0)
                {
                    if (!first) sbn.Append('|');
                    sbn.Append(UnitType.GetBaseUnitName(i));
                    if (!first) sbx.Append('|');
                    sbx.Append(this.baseUnitIndices[i]);
                    first = false;
                }
            }
            info.AddValue("names", sbn.ToString());
            info.AddValue("exps", sbx.ToString());
        }

        #endregion
    }
}
