using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RedStar.Amounts.JsonNet
{
    /// <summary>
    /// Converts an Amount to and from a JSON object, with two properties: value and unit.
    /// </summary>
    public class ObjectAmountJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var amount = value as Amount;

            writer.WriteStartObject();
            writer.WritePropertyName("value");
            writer.WriteValue(amount.Value);
            writer.WritePropertyName("unit");
            writer.WriteRawValue($"\"{amount.Unit.ToString(serializer.Culture)}\"");
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                var valueString = GetTokenValue(reader, "value");
                var unitString = GetTokenValue(reader, "unit");
                
                reader.Read();

                var value = double.Parse(valueString, serializer.Culture);
                var unit = Unit.Parse(unitString);
               
                return new Amount(value, unit);
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to deserialize at " + reader.Path + ".", ex);
            }
        }

        private static string GetTokenValue(JsonReader reader, string expectedPropertyName)
        {
            reader.Read();
            var propertyName = reader.Value != null ? reader.Value.ToString() : "";

            if (propertyName != expectedPropertyName)
            {
                throw new SerializationException($"Expected token '{expectedPropertyName}', but found '{propertyName}'.");
            }

            return reader.ReadAsString();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Amount);
        }
    }
}