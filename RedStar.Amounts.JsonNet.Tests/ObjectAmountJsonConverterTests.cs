using System.Globalization;
using Newtonsoft.Json;
using RedStar.Amounts.StandardUnits;
using Xunit;

namespace RedStar.Amounts.JsonNet.Tests
{
    public class ObjectAmountJsonConverterTests
    {
        private readonly JsonSerializerSettings _settings;

        public ObjectAmountJsonConverterTests()
        {
            UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);

            _settings = new JsonSerializerSettings
            {
                Culture = new CultureInfo("en"),
                Converters = new JsonConverter[]
                {
                    new ObjectAmountJsonConverter()
                }
            };
        }
        
        [Fact]
        public void WhenConvertingAmount_ReturnString()
        {
            var value = 3.4;
            var amount = new Amount(value, LengthUnits.Meter);

            var json = JsonConvert.SerializeObject(amount, _settings);

            Assert.Equal("{\"value\":3.4,\"unit\":\"m\"}", json);
        }

        [Fact]
        public void WhenConvertingJson_ReturnAmount()
        {
            var value = 3.4;
            var expected = new Amount(value, LengthUnits.Meter);
            var json = "{\"value\":3.4,\"unit\":\"m\"}";

            var actual = JsonConvert.DeserializeObject<Amount>(json, _settings);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WhenConvertingNull_ReturnNull()
        {
            var amount = (Amount)null;

            var json = JsonConvert.SerializeObject(amount, _settings);

            Assert.Equal("null", json);
        }

        [Fact]
        public void WhenConvertingNullJson_ReturnNull()
        {
            var json = "null";

            var actual = JsonConvert.DeserializeObject<Amount>(json, _settings);

            Assert.Null(actual);
        }

        [Fact]
        public void WhenConvertingCalculatedUnit_ReturnAmount()
        {
            var value = 3.4;
            var expected = new Amount(value, VolumeUnits.Meter3 / TimeUnits.Hour);
            var json = "{\"value\":3.4,\"unit\":\"m³/h\"}";

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

                Assert.Equal("{\"value\":0.0,\"unit\":\"°C/s\"}", json);
                Assert.Equal(new Amount(0, TemperatureUnits.DegreeCelcius / TimeUnits.Second), amount);
            }
        }

        [Fact]
        public void WhenConvertingComplexCalculatedUnit_ReturnAMount()
        {
            var value = 0.003;
            var jsonString = "{\"value\":0.003,\"unit\":\"GJ/10*Kg\"}";
            var expectedAmount = new Amount(value, EnergyUnits.GigaJoule / 10 * MassUnits.KiloGram);

            var actualAmount = JsonConvert.DeserializeObject<Amount>(jsonString, _settings);
            Assert.Equal(expectedAmount, actualAmount);
        }

        [Fact]
        public void WhenConvertingComplexCalculatedUnit_ReturnJson()
        {
            var value = 0.003;
            var expectedJsonString = "{\"value\":0.003,\"unit\":\"GJ/10*Kg\"}";
            var amount = new Amount(value, EnergyUnits.GigaJoule / 10 * MassUnits.KiloGram);

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