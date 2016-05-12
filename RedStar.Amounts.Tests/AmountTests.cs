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
    public class AmountTests
    {
        public AmountTests()
        {
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);
        }

        [Fact]
        public void Construction01Test()
        {
            var a = new Amount(100, "liter");
            Assert.Equal(100.0, a.Value);
            Assert.Equal("liter", a.Unit.Name);
        }

        [Fact]
        public void AdditionTest()
        {
            var a = new Amount(3000.0, LengthUnits.Meter);
            var sum = new Amount(2000.0, LengthUnits.Meter);
            var expected = new Amount(5000.0, LengthUnits.Meter);

            sum += a;

            Assert.Equal(expected, sum);
        }

        [Fact]
        public void AdditionDerivedTest()
        {
            var a = new Amount(3000.0, LengthUnits.Meter);
            var sum = new Amount(2.0, LengthUnits.KiloMeter);
            var expected = new Amount(5.0, LengthUnits.KiloMeter);

            sum += a;

            Assert.Equal(expected, sum);
        }

        [Fact]
        public void Conversion01Test()
        {
            var speed = new Amount(120, LengthUnits.KiloMeter / TimeUnits.Hour);
            var time = new Amount(15, TimeUnits.Minute);
            var distance = (speed * time).ConvertedTo(LengthUnits.KiloMeter, 4);
            Assert.Equal(30.0, distance.Value);
            Assert.Equal(LengthUnits.KiloMeter.Name, distance.Unit.Name);
        }

        [Fact]
        public void Casting01Test()
        {
            var a = (Amount)350.0;
            Assert.Equal(new Amount(350.0, Unit.None), a);

            var b = new Amount(123.0, Unit.None);
            Assert.Equal(123.0, (double)b);

            var c = new Amount(500.0, LengthUnits.Meter / LengthUnits.KiloMeter);
            Assert.Equal(0.5, (double)c);

            Assert.Equal("15.3", ((Amount)15.3).ToString().Replace(",", "."));
        }

        [Fact]
        public void Percentage01Test()
        {
            var percent = new Unit("percent", "%", 0.01 * Unit.None);

            var a = new Amount(15.0, percent);
            var b = new Amount(300.0, TimeUnits.Minute);

            Assert.Equal("15 %", a.ToString("0 US"));
            Assert.Equal(0.15, (double)a);
            Assert.Equal(45.0, (a * b).ConvertedTo(TimeUnits.Minute).Value);
        }

        [Fact]
        public void Percentage02Test()
        {
            var percent = new Unit("percent", "%", 0.01 * Unit.None);

            var a = new Amount(2.0, LengthUnits.Meter);
            var b = new Amount(17.0, LengthUnits.CentiMeter);

            var p = (b / a).ConvertedTo(percent);

            Assert.Equal("8.50 %", p.ToString("0.00 US", CultureInfo.InvariantCulture));
            Assert.Equal(0.085, (double)p);
        }

        [Fact]
        public void Power01Test()
        {
            var a = new Amount(12.0, LengthUnits.Meter);

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
            var a = new Amount(146.0, TimeUnits.Second);
            var values = a.Split(new[] { TimeUnits.Hour, TimeUnits.Minute, TimeUnits.Second }, 0);

            Assert.Equal(3, values.Length);
            Assert.Equal(new Amount(0.0, TimeUnits.Hour), values[0]);
            Assert.Equal(new Amount(2.0, TimeUnits.Minute), values[1]);
            Assert.Equal(new Amount(26.0, TimeUnits.Second), values[2]);
        }

        [Fact]
        public void Split02Test()
        {
            var a = new Amount(10.5, LengthUnits.Meter);
            var values = a.Split(new[] { LengthUnits.Yard, LengthUnits.Foot, LengthUnits.Inch }, 1);

            Assert.Equal(3, values.Length);
            Assert.Equal(new Amount(11.0, LengthUnits.Yard), values[0]);
            Assert.Equal(new Amount(1.0, LengthUnits.Foot), values[1]);
            Assert.Equal(new Amount(5.4, LengthUnits.Inch), values[2]);
        }

        [Fact]
        public void Split03Test()
        {
            var a = new Amount(Math.Sqrt(13), LengthUnits.Meter);
            var values = a.Split(new[] { LengthUnits.Meter, LengthUnits.DeciMeter, LengthUnits.CentiMeter, LengthUnits.MilliMeter }, 0);

            Assert.Equal(new Amount(3.0, LengthUnits.Meter), values[0]);
            Assert.Equal(new Amount(6.0, LengthUnits.DeciMeter), values[1]);
            Assert.Equal(new Amount(0.0, LengthUnits.CentiMeter), values[2]);
            Assert.Equal(new Amount(6.0, LengthUnits.MilliMeter), values[3]);
        }

        [Fact]
        public void Formatting01Test()
        {
            var defaultCultureInfo = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                var nlbe = CultureInfo.GetCultureInfo("nl-BE");
                var enus = CultureInfo.GetCultureInfo("en-US");

                var a = new Amount(12.3456789, LengthUnits.KiloMeter);
                var b = new Amount(12345.6789, LengthUnits.Meter);
                var c = new Amount(-0.45, LengthUnits.KiloMeter / TimeUnits.Hour);
                var d = new Amount(25.678, LengthUnits.Meter * LengthUnits.Meter);

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
            var b = new Amount(1234.5678, LengthUnits.Meter);

            Assert.Equal("", Amount.ToString(null, "#,##0.000 UN", CultureInfo.InvariantCulture));
            Assert.Equal("1,234.568 meter", Amount.ToString(b, "#,##0.000 UN", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Formatting03Test()
        {
            var d = new Amount(278.9, LengthUnits.Mile);
            var t = new Amount(2.5, TimeUnits.Hour);

            var s = d / t;

            Assert.Equal(
                "Taking 2,5 h to travel 449 km means your speed was 179,54 km/h",
                string.Format("Taking {1:GG|hour} to travel {0:#,##0 US|kilometer} means your speed was {2:#,##0.00 US|kilometer/hour}", d, t, s));

            Amount a = null;

            Assert.Equal("a = ", string.Format("a = {0:#,##0.0 US}", a));
        }

        [Fact]
        public void StaticFormattingTest()
        {
            var a = new Amount(1234.5678, LengthUnits.Meter);
            Amount b = null;

            var enus = CultureInfo.GetCultureInfo("en-US");
            var nlbe = CultureInfo.GetCultureInfo("nl-BE");

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
            var s = "";
            s = s + Amount.ToString(x, "#,##0.00 US|meter");

        }

        [Fact]
        public void SerializeDeserialize01Test()
        {
            var buffer = new MemoryStream();

            // Make some amounts:
            var a1before = new Amount(12345.6789, LengthUnits.Meter);
            var a2before = new Amount(-0.45, LengthUnits.KiloMeter / TimeUnits.Hour);

            // Serialize the units:
            var f = new BinaryFormatter();
            f.Serialize(buffer, a1before);
            f.Serialize(buffer, a2before);

            // Reset stream:
            buffer.Seek(0, SeekOrigin.Begin);

            // Deserialize units:
            var g = new BinaryFormatter();
            var a1after = (Amount)g.Deserialize(buffer);
            var a2after = (Amount)g.Deserialize(buffer);

            buffer.Close();

            Assert.Equal(a1before, a1after);
            Assert.Equal(a2before, a2after);
        }

        [Fact]
        public void NullAmountIsNotLessThanTest()
        {
            Amount a = null;
            var b = (Amount)100.0;

            Assert.Throws<NullReferenceException>(() => a < b);
        }

        [Fact]
        public void NullComparisonTest()
        {
            Amount a = null;
            var b = (Amount)100.0;

            int result = ((IComparable)b).CompareTo(a);

            Assert.True(result > 0);
        }

        [Fact]
        public void AdditionWithNullTest()
        {
            // Test both not null:
            var a = new Amount(100.0, LengthUnits.Meter);
            var b = new Amount(25.0, LengthUnits.Meter);
            var sum = a + b;
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
            // Test both not null:
            var a = new Amount(100.0, LengthUnits.Meter);
            var b = new Amount(25.0, LengthUnits.Meter);
            var subs = a - b;
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
            var a = new Amount(0.045, LengthUnits.Meter);
            var b = new Amount(0.0450000000001, LengthUnits.Meter);
            var c = new Amount(0.0450000000002, LengthUnits.Meter);
            var d = new Amount(0.046, LengthUnits.Meter);
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
            var a = new Amount(-0.00002, EnergyUnits.HorsePower);
            var b = new Amount(-0.00002, EnergyUnits.HorsePower);

            var ar = a.ConvertedTo(EnergyUnits.Watt);
            var br = b.ConvertedTo(EnergyUnits.Watt);

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
            var a = new Amount(120.0, SpeedUnits.KilometerPerHour);
            var b = new Amount(33.3333333330, SpeedUnits.MeterPerSecond);

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
            var d1 = new Amount(32.0, LengthUnits.KiloMeter);
            var d2 = new Amount(0.0, LengthUnits.KiloMeter);
            var t = new Amount(0.0, TimeUnits.Hour);

            Amount s;

            s = d1 / t;

            Assert.True(double.IsInfinity(s.Value));
            Assert.True(double.IsPositiveInfinity(s.Value));
            Assert.Equal(s.Unit, (d1.Unit / t.Unit));

            s = d2 / t;

            Assert.True(double.IsNaN(s.Value));
            Assert.Equal(s.Unit, (d2.Unit / t.Unit));
        }

        [Fact]
        public void AmountNetDataContractSerializerSerializationTest()
        {
            var a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            serializer.Serialize(stream, a);

            // Deserialize instance:
            stream.Position = 0;
            var b = (Amount)serializer.Deserialize(stream);

            // Compare:
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountArrayNetDataContractSerializerSerializationTest()
        {
            var aa = new Amount[6];
            aa[0] = new Amount(32.5, LengthUnits.NauticalMile);
            aa[1] = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);
            aa[2] = 3 * aa[0];
            aa[3] = 3 * aa[1];
            aa[4] = aa[1] / aa[3];
            aa[5] = new Amount(42.3, LengthUnits.Meter / TimeUnits.Second.Power(2));

            // Serialize instance:
            var stream = new MemoryStream();
            var serializer = new NetDataContractSerializer();
            serializer.WriteObject(stream, aa);

            // Deserialize instance:
            stream.Position = 0;
            var ba = (Amount[])serializer.ReadObject(stream);

            // Compare:
            Assert.Equal(aa.Length, ba.Length);
            for (var i = 0; i < aa.Length; i++)
            {
                Assert.Equal(aa[i], ba[i]);
            }
        }

        [Fact]
        public void AmountDataContractSerializerSerializationTest()
        {
            var a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            var stream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(Amount));
            serializer.WriteObject(stream, a);

            // Deserialize instance:
            stream.Position = 0;
            var b = (Amount)serializer.ReadObject(stream);

            // Compare:
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountArrayDataContractSerializerSerializationTest()
        {
            var aa = new Amount[6];
            aa[0] = new Amount(32.5, LengthUnits.NauticalMile);
            aa[1] = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);
            aa[2] = 3 * aa[0];
            aa[3] = 3 * aa[1];
            aa[4] = aa[1] / aa[3];
            aa[5] = new Amount(42.3, LengthUnits.Meter / TimeUnits.Second.Power(2));

            // Serialize instance:
            var stream = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(Amount[]));
            serializer.WriteObject(stream, aa);

            // Deserialize instance:
            stream.Position = 0;
            var ba = (Amount[])serializer.ReadObject(stream);

            // Compare:
            Assert.Equal(aa.Length, ba.Length);
            for (var i = 0; i < aa.Length; i++)
            {
                Assert.Equal(aa[i], ba[i]);
            }
        }

        [Fact]
        public void AmountBinaryFormatterSerializationTest()
        {
            var a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, a);

            // Deserialize instance:
            stream.Position = 0;
            var b = (Amount)formatter.Deserialize(stream);

            // Compare:
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountSoapFormatterSerializationTest()
        {
            var a = new Amount(3500.12, EnergyUnits.KiloWattHour * (365.0 * TimeUnits.Day) / VolumeUnits.Meter3);

            // Serialize instance:
            var stream = new MemoryStream();
            var formatter = new SoapFormatter();
            formatter.Serialize(stream, a);

            // Deserialize instance:
            stream.Position = 0;
            var b = (Amount)formatter.Deserialize(stream);

            // Compare:
            Assert.Equal(a, b);
        }

        [Fact]
        public void AmountCompatibilityTest()
        {
            var a = new Amount(300, LengthUnits.Mile / TimeUnits.Hour.Power(2));
            Assert.True(a.Unit.IsCompatibleTo(LengthUnits.Meter / TimeUnits.Second.Power(2)));
            Assert.True(a.Unit.IsCompatibleTo(LengthUnits.Meter * TimeUnits.Second.Power(-2)));
            Assert.False(a.Unit.IsCompatibleTo(LengthUnits.Meter / TimeUnits.Second.Power(1)));
            Assert.False(a.Unit.IsCompatibleTo(MassUnits.Gram));
        }

        [Fact]
        public void AmountSplitTest()
        {
            // One fifth of a week:
            var a = new Amount(1.0 / 5.0, TimeUnits.Day * 7.0);

            var result = a.Split(new[] { TimeUnits.Day, TimeUnits.Hour, TimeUnits.Minute, TimeUnits.Second }, 3);

            Assert.Equal(4, result.Length);
            Assert.Equal(new[] { 1.0, 9.0, 36.0, 0.0 }.ToList(), result.Select(x => x.Value).ToList());
        }

        [Fact]
        public void AmountSplit2Test()
        {
            // One fifth of a week:
            var a = new Amount(7.0 / 5.0, TimeUnits.Day);

            var result = a.Split(new[] { TimeUnits.Day, TimeUnits.Hour, TimeUnits.Minute, TimeUnits.Second }, 3);

            // In this case, the split results in 1 day, 9 hours, 35 minutes and 60 SECONDS!
            // This is due to rounding; it results in ..., 35 minutes and 59.99999 seconds,
            // which once rounded, end up to be 60 seconds...

            Assert.Equal(4, result.Length);
            Assert.Equal(new[] { 1.0, 9.0, 35.0, 60.0 }.ToList(), result.Select(x => x.Value).ToList());
        }

        [Fact]
        public void AmountSplitIncompatibleTest()
        {
            // One fifth of a week:
            var a = new Amount(7.0 / 5.0, TimeUnits.Day);

            Assert.Throws<UnitConversionException>(() => a.Split(new[] { TimeUnits.Day, TimeUnits.Hour, LengthUnits.Meter, TimeUnits.Second }, 3));
        }

        [Fact]
        public void Parsing01Test()
        {
            var defaultCultureInfo = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                var nlbe = CultureInfo.GetCultureInfo("nl-BE");
                var enus = CultureInfo.GetCultureInfo("en-US");

                var a = new Amount(12.3456789, LengthUnits.KiloMeter);
                var b = new Amount(12345.6789, LengthUnits.Meter);
                var c = new Amount(-0.45, LengthUnits.KiloMeter / TimeUnits.Hour);
                Assert.Equal(a, Amount.Parse("12.3456789 km"));
                Assert.Equal(a, Amount.Parse("12,3456789 kilometer", nlbe));
                Assert.Equal(b, Amount.Parse("12.345,6789 m", nlbe));
                Assert.Equal(b, Amount.Parse("12,345.6789 m", enus));
                Assert.Equal(c, Amount.Parse("-0.45 km/h", enus));
                Assert.Equal(c, Amount.Parse("-0.45 (kilometer/hour)", enus));
                Assert.Equal(c, Amount.Parse("-0,450 km/h", nlbe));
                Assert.Equal(-b, Amount.Parse("12.3456789 km neg"));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = defaultCultureInfo;
            }
        }

        [Fact]
        public void Parsing02Test()
        {
            var b = new Amount(1234.5678, LengthUnits.Meter);

            Assert.Equal(null, Amount.Parse("", CultureInfo.InvariantCulture));
            Assert.Equal(b, Amount.Parse("1,234.5678 meter", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Parsing03Test()
        {
            var b = new Amount(0, Unit.None);

            Assert.Equal(b, Amount.Parse("0", CultureInfo.InvariantCulture));
        }
    }
}
