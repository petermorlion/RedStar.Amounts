using System.Collections.Generic;
using System.Text;

namespace RedStar.Amounts
{
    internal class UnitParser
    {
        internal static Unit Parse(string s)
        {
            var result = Unit.None;
            if (string.IsNullOrEmpty(s))
                return result;

            s = s.TrimStart('(').TrimEnd(')');
            while (s.Contains("**"))
            {
                s = s.Replace("**", "*");
            }

            while (s.Contains("//"))
            {
                s = s.Replace("//", "/");
            }

            var unitParts = new List<UnitPart>();
            var unitPartBuilder = new StringBuilder();
            foreach (var character in s)
            {
                if (character == '*' || character == '/')
                {
                    var unitPart = new UnitPart(unitPartBuilder.ToString());
                    unitParts.Add(unitPart);
                    unitParts.Add(new UnitPart(character.ToString()));
                    unitPartBuilder = new StringBuilder();
                }
                else
                {
                    unitPartBuilder.Append(character);
                }
            }

            unitParts.Add(new UnitPart(unitPartBuilder.ToString()));

            if (unitParts.Count == 1)
                return unitParts[0].AsUnit();

            double initialMultiplyer = 1;
            for (var i = 0; i < unitParts.Count; i++)
            {
                var unitPart = unitParts[i];

                if (i == 0)
                {
                    if (unitPart.UnitPartType == UnitPartType.Unit)
                    {
                        result = unitPart.AsUnit();
                    }
                    else if (unitPart.UnitPartType == UnitPartType.Numeric)
                    {
                        initialMultiplyer = unitPart.AsDouble();
                    }
                }
                else
                {
                    if (unitPart.UnitPartType == UnitPartType.Divider)
                    {
                        i++;
                        var nextUnitPart = unitParts[i];
                        if (nextUnitPart.UnitPartType == UnitPartType.Numeric)
                        {
                            result = result / nextUnitPart.AsDouble();
                        }
                        else
                        {
                            result = result / nextUnitPart.AsUnit();
                        }
                    }
                    else if (unitPart.UnitPartType == UnitPartType.Multiplyer)
                    {
                        i++;
                        var nextUnitPart = unitParts[i];
                        if (nextUnitPart.UnitPartType == UnitPartType.Numeric)
                        {
                            result = result * nextUnitPart.AsDouble();
                        }
                        else
                        {
                            result = result * nextUnitPart.AsUnit();
                        }
                    }
                }
            }

            return initialMultiplyer * result;
        }

        private class UnitPart
        {
            private readonly string _value;

            internal UnitPart(string s)
            {
                _value = s;

                double doubleValue;
                if (double.TryParse(s, out doubleValue))
                {
                    UnitPartType = UnitPartType.Numeric;
                }
                else if (s == "*")
                {
                    UnitPartType = UnitPartType.Multiplyer;
                }
                else if (s == "/")
                {
                    UnitPartType = UnitPartType.Divider;
                }
                else
                {
                    UnitPartType = UnitPartType.Unit;
                }
            }

            internal double AsDouble()
            {
                return UnitPartType == UnitPartType.Numeric ? double.Parse(_value) : 1;
            }

            internal Unit AsUnit()
            {
                Unit result;
                if (!UnitManager.TryGetUnitByName(_value, out result))
                {
                    result = UnitManager.GetUnitBySymbol(_value);
                }

                return result;
            }

            internal UnitPartType UnitPartType { get; }
        }

        private enum UnitPartType
        {
            Unit,
            Multiplyer,
            Divider,
            Numeric
        }
    }
}