using System.Globalization;
using Newtonsoft.Json;
using RedStar.Amounts.StandardUnits;
using Xunit;

namespace RedStar.Amounts.JsonNet.Tests
{
    public class StringAmountJsonConverterTests
    {
        private readonly JsonSerializerSettings _settings;

        public StringAmountJsonConverterTests()
        {
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);

            _settings = new JsonSerializerSettings
            {
                Culture = new CultureInfo("en"),
                Converters = new JsonConverter[]
                {
                    new StringAmountJsonConverter()
                }
            };
        }

        [Fact]
        public void WhenConvertingAmount_ReturnString()
        {
            var amount = new Amount(30, MassUnits.KiloGram);

            var result = JsonConvert.SerializeObject(amount, _settings);

            Assert.Equal("\"30 Kg\"", result);
        }

        [Fact]
        public void WhenConvertingNull_ReturnNull()
        {
            var result = JsonConvert.SerializeObject(null, _settings);

            Assert.Equal("null", result);
        }

        [Fact]
        public void WhenConvertingJson_ReturnAmount()
        {
            var json = "\"3.4 Kg\"";

            var result = JsonConvert.DeserializeObject<Amount>(json, _settings);

            Assert.Equal(new Amount(3.4, MassUnits.KiloGram), result);
        }

        [Fact]
        public void WhenConvertingNullJson_ReturnNull()
        {
            var json = "null";

            var result = JsonConvert.DeserializeObject<Amount>(json, _settings);

            Assert.Null(result);
        }

        [Fact]
        public void WhenConvertingCalculatedUnit_ReturnAmount()
        {
            var expected = new Amount(3.4, VolumeUnits.Meter3 / TimeUnits.Hour);
            var json = "\"3.4 m³/h\"";

            var actual = JsonConvert.DeserializeObject<Amount>(json, _settings);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenConvertingMultipleTimes_KeepValuesConsistent()
        {
            var amount = new Amount(0, TemperatureUnits.DegreeCelcius / TimeUnits.Second);

            for (var i = 0; i < 10; i++)
            {
                var json = JsonConvert.SerializeObject(amount, _settings);
                amount = JsonConvert.DeserializeObject<Amount>(json, _settings);

                Assert.Equal("\"0 °C/s\"", json);
                Assert.Equal(new Amount(0, TemperatureUnits.DegreeCelcius / TimeUnits.Second), amount);
            }
        }

        [Fact]
        public void WhenConvertingComplexCalculatedUnit_ReturnAMount()
        {
            var jsonString = "\"0.003 GJ/10*Kg\"";
            var expectedAmount = new Amount(0.003, EnergyUnits.GigaJoule / 10 * MassUnits.KiloGram);

            var actualAmount = JsonConvert.DeserializeObject<Amount>(jsonString, _settings);
            Assert.Equal(expectedAmount, actualAmount);
        }

        [Fact]
        public void WhenConvertingComplexCalculatedUnit_ReturnJson()
        {
            var expectedJsonString = "\"0.003 GJ/10*Kg\"";
            var amount = new Amount(0.003, EnergyUnits.GigaJoule / 10 * MassUnits.KiloGram);

            var actualJsonString = JsonConvert.SerializeObject(amount, _settings);
            Assert.Equal(expectedJsonString, actualJsonString);
        }

        [Fact]
        public void WhenConvertingNullProperty_ReturnJson()
        {
            var obj = new MyClass();
            var expectedJsonString = "{\"MyProperty\":null}";

            var actualJsonString = JsonConvert.SerializeObject(obj, _settings);
            Assert.Equal(expectedJsonString, actualJsonString);
        }

        [Fact]
        public void WhenConvertingNullJson_ReturnObjectWithNullProperty()
        {
            var jsonString = "{\"MyProperty\":null}";

            var newObj = JsonConvert.DeserializeObject<MyClass>(jsonString, _settings);
            Assert.Null(newObj.MyProperty);
        }

        private class MyClass
        {
            public Amount MyProperty { get; set; }
        }
    }
}
