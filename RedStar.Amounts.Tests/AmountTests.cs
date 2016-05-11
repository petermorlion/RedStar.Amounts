using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Threading;
using RedStar.Amounts.StandardUnits;
using Xunit;

namespace RedStar.Amounts.Tests
{
    public class AmountTests : IDisposable
    {
        private UnitManager defaultUnitManager;

        public AmountTests()
        {
            this.defaultUnitManager = UnitManager.Instance;
            UnitManager.Instance = new UnitManager();
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);
        }

        public void Dispose()
        {
            UnitManager.Instance = this.defaultUnitManager;
        }

        [Fact]
        public void Construction01Test()
        {
            Amount a = new Amount(100, "liter");
            Assert.Equal(100.0, a.Value);
            Assert.Equal("liter", a.Unit.Name);
        }

        [Fact]
        public void AdditionTest()
        {
            Amount a = new Amount(3000.0, LengthUnits.Meter);
            Amount sum = new Amount(2000.0, LengthUnits.Meter);
            Amount expected = new Amount(5000.0, LengthUnits.Meter);

            sum += a;

            Console.WriteLine("Sum = {0}", sum);
            Assert.Equal(expected, sum);
        }

        [Fact]
        public void AdditionDerivedTest()
        {
            Amount a = new Amount(3000.0, LengthUnits.Meter);
            Amount sum = new Amount(2.0, LengthUnits.KiloMeter);
            Amount expected = new Amount(5.0, LengthUnits.KiloMeter);

            sum += a;

            Console.WriteLine("Sum = {0}", sum);
            Assert.Equal(expected, sum);
        }

        [Fact]
        public void Conversion01Test()
        {
            Amount speed = new Amount(120, LengthUnits.KiloMeter / TimeUnits.Hour);
            Amount time = new Amount(15, TimeUnits.Minute);
            Amount distance = (speed * time).ConvertedTo(LengthUnits.KiloMeter, 4);
            Assert.Equal(30.0, distance.Value);
            Assert.Equal(LengthUnits.KiloMeter.Name, distance.Unit.Name);
        }

        [Fact]
        public void Casting01Test()
        {
            Amount a = (Amount)350.0;
            Assert.Equal(new Amount(350.0, Unit.None), a);

            Amount b = new Amount(123.0, Unit.None);
            Assert.Equal(123.0, (double)b);

            Amount c = new Amount(500.0, LengthUnits.Meter / LengthUnits.KiloMeter);
            Assert.Equal(0.5, (double)c);

            Assert.Equal("15.3", ((Amount)15.3).ToString().Replace(",", "."));
        }

        [Fact]
        public void Percentage01Test()
        {
            Unit percent = new Unit("percent", "%", 0.01 * Unit.None);

            Amount a = new Amount(15.0, percent);
            Amount b = new Amount(300.0, TimeUnits.Minute);

            Assert.Equal("15 %", a.ToString("0 US"));
            Assert.Equal(0.15, (double)a);
            Console.WriteLine(a * b);
            Assert.Equal(45.0, (a * b).ConvertedTo(TimeUnits.Minute).Value);
        }

        [Fact]
        public void Percentage02Test()
        {
            Unit percent = new Unit("percent", "%", 0.01 * Unit.None);

            Amount a = new Amount(2.0, LengthUnits.Meter);
            Amount b = new Amount(17.0, LengthUnits.CentiMeter);

            Amount p = (b / a).ConvertedTo(percent);

            Assert.Equal("8.50 %", p.ToString("0.00 US", CultureInfo.InvariantCulture));
            Assert.Equal(0.085, (double)p);
        }

        [Fact]
        public void Power01Test()
        {
            Amount a = new Amount(12.0, LengthUnits.Meter);

            Assert.Equal(new Amount(1.0, Unit.None), a.Power(0));

            Assert.Equal(new Amount(12.0, LengthUnits.Meter), a.Power(1));
            Assert.Equal(new Amount(144.0, SurfaceUnits.Meter2), a.Power(2));
            Assert.Equal(new Amount(1728.0, VolumeUnits.Meter3), a.Power(3));

            Assert.Equal(new Amount(1.0 / 12.0, Unit.None / LengthUnits.Meter), a.Power(-1));
            Assert.Equal(new Amount(1.0 / 144.0, Unit.None / SurfaceUnits.Meter2), a.Power(-2));
            Assert.Equal(new Amount(1.0 / 1728.0, Unit.None / VolumeUnits.Meter3), a.Power(-3));
        }

        [Fact]
        public void Split01Test()
        {
            Amount a = new Amount(146.0, TimeUnits.Second);
            Amount[] values = a.Split(new Unit[] { TimeUnits.Hour, TimeUnits.Minute, TimeUnits.Second }, 0);

            string separator = "";
            foreach (Amount v in values)
            {
                Console.Write(separator);
                Console.Write(v);
                separator = ", ";
            }
            Console.WriteLine();

            Assert.Equal(3, values.Length);
            Assert.Equal(new Amount(0.0, TimeUnits.Hour), values[0]);
            Assert.Equal(new Amount(2.0, TimeUnits.Minute), values[1]);
            Assert.Equal(new Amount(26.0, TimeUnits.Second), values[2]);
        }

        [Fact]
        public void Split02Test()
        {
            Amount a = new Amount(10.5, LengthUnits.Meter);
            Amount[] values = a.Split(new Unit[] { LengthUnits.Yard, LengthUnits.Foot, LengthUnits.Inch }, 1);

            string separator = "";
            foreach (Amount v in values)
            {
                Console.Write(separator);
                Console.Write(v);
                separator = ", ";
            }
            Console.WriteLine();

            Assert.Equal(3, values.Length);
            Assert.Equal(new Amount(11.0, LengthUnits.Yard), values[0]);
            Assert.Equal(new Amount(1.0, LengthUnits.Foot), values[1]);
            Assert.Equal(new Amount(5.4, LengthUnits.Inch), values[2]);
        }

        [Fact]
        public void Split03Test()
        {
            Amount a = new Amount(global::System.Math.Sqrt(13), LengthUnits.Meter);
            Amount[] values = a.Split(new Unit[] { LengthUnits.Meter, LengthUnits.DeciMeter, LengthUnits.CentiMeter, LengthUnits.MilliMeter }, 0);

            string separator = "";
            foreach (Amount v in values)
            {
                Console.Write(separator);
                Console.Write(v);
                separator = ", ";
            }
            Console.WriteLine();

            Assert.Equal(new Amount(3.0, LengthUnits.Meter), values[0]);
            Assert.Equal(new Amount(6.0, LengthUnits.DeciMeter), values[1]);
            Assert.Equal(new Amount(0.0, LengthUnits.CentiMeter), values[2]);
            Assert.Equal(new Amount(6.0, LengthUnits.MilliMeter), values[3]);
        }

        [Fact]
        public void Formatting01Test()
        {
            CultureInfo defaultCultureInfo = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                CultureInfo nlbe = CultureInfo.GetCultureInfo("nl-BE");
                CultureInfo enus = CultureInfo.GetCultureInfo("en-US");

                Amount a = new Amount(12.3456789, LengthUnits.KiloMeter);
                Amount b = new Amount(12345.6789, LengthUnits.Meter);
                Amount c = new Amount(-0.45, LengthUnits.KiloMeter / TimeUnits.Hour);
                Amount d = new Amount(25.678, LengthUnits.Meter * LengthUnits.Meter);

                Assert.Equal("12.3456789 km", a.ToString());
                Assert.Equal("12,3456789 kilometer", a.ToString("GN", nlbe));
                Assert.Equal("12,35 km", a.ToString("NS", nlbe));
                Assert.Equal("12.35 km", a.ToString("NS", enus));
                Assert.Equal("12.345,68 m", b.ToString("NS", nlbe));
                Assert.Equal("12,345.68 m", b.ToString("NS", enus));
                Assert.Equal("-0.45 km/h", c.ToString("NS", enus));
                Assert.Equal("-0.45 (kilometer/hour)", c.ToString("NN", enus));
                Assert.Equal("-0,450 km/h", c.ToString("0.000 US", nlbe));
                Assert.Equal("[0,450] km/h", c.ToString("0.000 US;[0.000] US", nlbe));
                Assert.Equal("12.35 kilometer", b.ToString("NN|kilometer"));
                Assert.Equal("12.346 km", b.ToString("#,##0.000 US|kilometer"));
                Assert.Equal("+12.346 km", b.ToString("+#,##0.000 US|kilometer"));
                Assert.Equal("12.346 km neg", (-b).ToString("#,##0.000 US pos;#,##0.000 US neg|kilometer"));
                Assert.Equal("25.68 m*m", d.ToString("NS"));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = defaultCultureInfo;
            }
        }

        [Fact]
        public void Formatting02Test()
        {
            Amount b = new Amount(1234.5678, LengthUnits.Meter);

            Assert.Equal("", Amount.ToString(null, "#,##0.000 UN", CultureInfo.InvariantCulture));
            Assert.Equal("1,234.568 meter", Amount.ToString(b, "#,##0.000 UN", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Formatting03Test()
        {
            Amount d = new Amount(278.9, LengthUnits.Mile);
            Amount t = new Amount(2.5, TimeUnits.Hour);

            var s = d / t;

            Assert.Equal(
                "Taking 2,5 h to travel 449 km means your speed was 179,54 km/h",
                String.Format("Taking {1:GG|hour} to travel {0:#,##0 US|kilometer} means your speed was {2:#,##0.00 US|kilometer/hour}", d, t, s));

            Amount a = null;

            Assert.Equal("a = ", String.Format("a = {0:#,##0.0 US}", a));
        }

        [Fact]
        public void StaticFormattingTest()
        {
            Amount a = new Amount(1234.5678, LengthUnits.Meter);
            Amount b = null;

            CultureInfo enus = CultureInfo.GetCultureInfo("en-US");
            CultureInfo nlbe = CultureInfo.GetCultureInfo("nl-BE");

            Assert.Equal("1234.5678 m", Amount.ToString(a).Replace(",", "."));
            Assert.Equal("1234.5678 m", Amount.ToString(a, enus));
            Assert.Equal("1234,5678 m", Amount.ToString(a, nlbe));
            Assert.Equal("1.234.57 m", Amount.ToString(a, "#,##0.00 US").Replace(",", "."));
            Assert.Equal("1,234.57 m", Amount.ToString(a, "#,##0.00 US", enus));
            Assert.Equal("1.234,57 m", Amount.ToString(a, "#,##0.00 US", nlbe));

            Assert.Equal("", Amount.ToString(b).Replace(",", "."));
            Assert.Equal("", Amount.ToString(b, enus));
            Assert.Equal("", Amount.ToString(b, nlbe));
            Assert.Equal("", Amount.ToString(b, "#,##0.00 US").Replace(",", "."));
            Assert.Equal("", Amount.ToString(b, "#,##0.00 US", enus));
            Assert.Equal("", Amount.ToString(b, "#,##0.00 US", nlbe));

            Amount x = null;
            String s = "";
            s = s + Amount.ToString(x, "#,##0.00 US|meter");

        }

        [Fact]
        public void SerializeDeserialize01Test()
        {
            MemoryStream buffer = new MemoryStream();

            // Make some amounts:
            Amount a1before = new Amount(12345.6789, LengthUnits.Meter);
            Amount a2before = new Amount(-0.45, LengthUnits.KiloMeter / TimeUnits.Hour);

            // Serialize the units:
            BinaryFormatter f = new BinaryFormatter();
            f.Serialize(buffer, a1before);
            f.Serialize(buffer, a2before);

            // Reset stream:
            buffer.Seek(0, SeekOrigin.Begin);

            // Deserialize units:
            BinaryFormatter g = new BinaryFormatter();
            Amount a1after = (Amount)g.Deserialize(buffer);
            Amount a2after = (Amount)g.Deserialize(buffer);

            buffer.Close();

            Console.WriteLine("{0} => {1}", a1before, a1after);
            Console.WriteLine("{0} => {1}", a2before, a2after);

            Assert.Equal(a1before, a1after);
            Assert.Equal(a2before, a2after);
        }

        [Fact]
        public void NullAmountIsNotLessThanTest()
        {
            Amount a = null;
            Amount b = (Amount)100.0;

            Assert.Throws<NullReferenceException>(() => a < b);
        }

        [Fact]
        public void NullComparisonTest()
        {
            Amount a = null;
            Amount b = (Amount)100.0;

            int result = ((IComparable)b).CompareTo(a);

            Assert.True(result > 0);
        }

        [Fact]
        public void AdditionWithNullTest()
        {
            Amount a, b, sum;

            // Test both not null:
            a = new Amount(100.0, LengthUnits.Meter);
            b = new Amount(25.0, LengthUnits.Meter);
            sum = a + b;
            Assert.Equal(new Amount(125.0, LengthUnits.Meter), sum);

            // Test right not null:
            a = new Amount(100.0, LengthUnits.Meter);
            b = null;
            sum = a + b;
            Assert.Equal(new Amount(100.0, LengthUnits.Meter), sum);

            // Test left not null:
            a = null;
            b = new Amount(25.0, LengthUnits.Meter);
            sum = a + b;
            Assert.Equal(new Amount(25.0, LengthUnits.Meter), sum);

            // Test both null:
            a = null;
            b = null;
            sum = a + b;
            Assert.Equal(null, sum);
        }

        [Fact]
        public void SubstractWithNullTest()
        {
            Amount a, b, subs;

            // Test both not null:
            a = new Amount(100.0, LengthUnits.Meter);
            b = new Amount(25.0, LengthUnits.Meter);
            subs = a - b;
            Assert.Equal(new Amount(75.0, LengthUnits.Meter), subs);

            // Test right not null:
            a = new Amount(100.0, LengthUnits.Meter);
            b = null;
            subs = a - b;
            Assert.Equal(new Amount(100.0, LengthUnits.Meter), subs);

            // Test left not null:
            a = null;
            b = new Amount(25.0, LengthUnits.Meter);
            subs = a - b;
            Assert.Equal(new Amount(-25.0, LengthUnits.Meter), subs);

            // Test both null:
            a = null;
            b = null;
            subs = a - b;
            Assert.Equal(null, subs);
        }

        [Fact]
        public void RoundedComparisonTest()
        {
            Amount a = new Amount(0.045, LengthUnits.Meter);
            Amount b = new Amount(0.0450000000001, LengthUnits.Meter);
            Amount c = new Amount(0.0450000000002, LengthUnits.Meter);
            Amount d = new Amount(0.046, LengthUnits.Meter);
            Assert.False(a.Value == b.Value);
            Assert.False(b.Value == c.Value);
            Assert.False(a.Value == c.Value);
            Assert.True(a == b);
            Assert.True(b == c);
            Assert.True(a == c);
            Assert.False(c == d);
            Assert.True(a.Equals(b));
            Assert.True(b.Equals(c));
            Assert.True(a.Equals(c));
            Assert.False(c.Equals(d));
        }

        [Fact]
        public void Comparison01Test()
        {
            Amount a = new Amount(-0.00002, EnergyUnits.HorsePower);
            Amount b = new Amount(-0.00002, EnergyUnits.HorsePower);

            Amount ar = a.ConvertedTo(EnergyUnits.Watt);
            Amount br = b.ConvertedTo(EnergyUnits.Watt);

            Assert.True(a == b);
            Assert.False(a > b);
            Assert.False(a < b);
            Assert.True(ar == br);
            Assert.False(ar > br);
            Assert.False(ar < br);
        }

        [Fact]
        public void Comparison02Test()
        {
            Amount a = new Amount(120.0, SpeedUnits.KilometerPerHour);
            Amount b = new Amount(33.3333333330, SpeedUnits.MeterPerSecond);

            Assert.True(a == b);
            Assert.False(a < b);
            Assert.False(a > b);
            Assert.True(a <= b);
            Assert.True(a >= b);
            Assert.False(a != b);
        }

        [Fact]
        public void DivisionByZeroTest()
        {
            Amount d1 = new Amount(32.0, LengthUnits.KiloMeter);
            Amount d2 = new Amount(0.0, LengthUnits.KiloMeter);
            Amount t = new Amount(0.0, TimeUnits.Hour);

            Amount s;

            s = d1 / t;

            Assert.True(Double.IsInfinity(s.Value));
            Assert.True(Double.IsPositiveInfinity(s.Value));
            Assert.Equal(s.Unit, (d1.Unit / t.Unit));

            s = d2 / t;

            Assert.True(Double.IsNaN(s.Value));
            Assert.Equal(s.Unit, (d2.Unit / t.Unit));
        }

        [Fact]
        public void AmountNetDataContractSerializerSerializationTest()
        {
            Amount a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            MemoryStream stream = new MemoryStream();
            NetDataContractSerializer serializer = new NetDataContractSerializer();
            serializer.Serialize(stream, a);

            // Show serialization:
            stream.Position = 0;
            Console.WriteLine(stream.ToXmlString());
            Console.WriteLine();

            // Deserialize instance:
            stream.Position = 0;
            Amount b = (Amount)serializer.Deserialize(stream);

            // Compare:
            Console.WriteLine(a);
            Console.WriteLine(b);
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountArrayNetDataContractSerializerSerializationTest()
        {
            Amount[] aa = new Amount[6];
            aa[0] = new Amount(32.5, LengthUnits.NauticalMile);
            aa[1] = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);
            aa[2] = 3 * aa[0];
            aa[3] = 3 * aa[1];
            aa[4] = aa[1] / aa[3];
            aa[5] = new Amount(42.3, LengthUnits.Meter / TimeUnits.Second.Power(2));

            // Serialize instance:
            MemoryStream stream = new MemoryStream();
            NetDataContractSerializer serializer = new NetDataContractSerializer();
            serializer.WriteObject(stream, aa);

            // Show serialization:
            stream.Position = 0;
            Console.WriteLine(stream.ToXmlString());
            Console.WriteLine();

            // Deserialize instance:
            stream.Position = 0;
            Amount[] ba = (Amount[])serializer.ReadObject(stream);

            // Compare:
            Assert.Equal(aa.Length, ba.Length);
            for (int i = 0; i < aa.Length; i++)
            {
                Console.WriteLine(aa[i]);
                Console.WriteLine(ba[i]);
                Assert.Equal(aa[i], ba[i]);
            }
        }

        [Fact]
        public void AmountDataContractSerializerSerializationTest()
        {
            Amount a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            MemoryStream stream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(Amount));
            serializer.WriteObject(stream, a);

            // Show serialization:
            stream.Position = 0;
            Console.WriteLine(stream.ToXmlString());
            Console.WriteLine();

            // Deserialize instance:
            stream.Position = 0;
            Amount b = (Amount)serializer.ReadObject(stream);

            // Compare:
            Console.WriteLine(a);
            Console.WriteLine(b);
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountArrayDataContractSerializerSerializationTest()
        {
            Amount[] aa = new Amount[6];
            aa[0] = new Amount(32.5, LengthUnits.NauticalMile);
            aa[1] = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);
            aa[2] = 3 * aa[0];
            aa[3] = 3 * aa[1];
            aa[4] = aa[1] / aa[3];
            aa[5] = new Amount(42.3, LengthUnits.Meter / TimeUnits.Second.Power(2));

            // Serialize instance:
            MemoryStream stream = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(Amount[]));
            serializer.WriteObject(stream, aa);

            // Show serialization:
            stream.Position = 0;
            Console.WriteLine(stream.ToXmlString());
            Console.WriteLine();

            // Deserialize instance:
            stream.Position = 0;
            Amount[] ba = (Amount[])serializer.ReadObject(stream);

            // Compare:
            Assert.Equal(aa.Length, ba.Length);
            for (int i = 0; i < aa.Length; i++)
            {
                Console.WriteLine(aa[i]);
                Console.WriteLine(ba[i]);
                Assert.Equal(aa[i], ba[i]);
            }
        }

        [Fact]
        public void AmountBinaryFormatterSerializationTest()
        {
            Amount a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);

            // Deserialize instance:
            stream.Position = 0;
            Amount b = (Amount)formatter.Deserialize(stream);

            // Compare:
            Console.WriteLine(a);
            Console.WriteLine(b);
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountSoapFormatterSerializationTest()
        {
            Amount a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            MemoryStream stream = new MemoryStream();
            SoapFormatter formatter = new SoapFormatter();
            formatter.Serialize(stream, a);

            // Show serialization:
            stream.Position = 0;
            Console.WriteLine(stream.ToXmlString());
            Console.WriteLine();

            // Deserialize instance:
            stream.Position = 0;
            Amount b = (Amount)formatter.Deserialize(stream);

            // Compare:
            Console.WriteLine(a);
            Console.WriteLine(b);
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountCompatibilityTest()
        {
            Amount a = new Amount(300, LengthUnits.Mile / TimeUnits.Hour.Power(2));
            Assert.True(a.Unit.IsCompatibleTo(LengthUnits.Meter / TimeUnits.Second.Power(2)));
            Assert.True(a.Unit.IsCompatibleTo(LengthUnits.Meter * TimeUnits.Second.Power(-2)));
            Assert.False(a.Unit.IsCompatibleTo(LengthUnits.Meter / TimeUnits.Second.Power(1)));
            Assert.False(a.Unit.IsCompatibleTo(MassUnits.Gram));
            Console.WriteLine(a.Unit.UnitType.ToString());
        }

        [Fact]
        public void AmountSplitTest()
        {
            // One fifth of a week:
            Amount a = new Amount(1.0 / 5.0, TimeUnits.Day * 7.0);

            Amount[] result = a.Split(new Unit[] { TimeUnits.Day, TimeUnits.Hour, TimeUnits.Minute, TimeUnits.Second }, 3);

            foreach (var item in result)
                Console.WriteLine(item);

            Assert.Equal(4, result.Length);
            Assert.Equal(new double[] { 1.0, 9.0, 36.0, 0.0 }.ToList(), result.Select(x => x.Value).ToList());
        }

        [Fact]
        public void AmountSplit2Test()
        {
            // One fifth of a week:
            Amount a = new Amount(7.0 / 5.0, TimeUnits.Day);

            Amount[] result = a.Split(new Unit[] { TimeUnits.Day, TimeUnits.Hour, TimeUnits.Minute, TimeUnits.Second }, 3);

            foreach (var item in result)
                Console.WriteLine(item);

            // In this case, the split results in 1 day, 9 hours, 35 minutes and 60 SECONDS!
            // This is due to rounding; it results in ..., 35 minutes and 59.99999 seconds,
            // which once rounded, end up to be 60 seconds...

            Assert.Equal(4, result.Length);
            Assert.Equal(new double[] { 1.0, 9.0, 35.0, 60.0 }.ToList(), result.Select(x => x.Value).ToList());
        }

        [Fact]
        public void AmountSplitIncompatibleTest()
        {
            // One fifth of a week:
            Amount a = new Amount(7.0 / 5.0, TimeUnits.Day);

            Assert.Throws<UnitConversionException>(() => a.Split(new Unit[] { TimeUnits.Day, TimeUnits.Hour, LengthUnits.Meter, TimeUnits.Second }, 3));
        }
    }
}
