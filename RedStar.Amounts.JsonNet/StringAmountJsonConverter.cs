using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RedStar.Amounts.JsonNet
{
    /// <summary>
    /// Converts an Amount from and to a string.
    /// </summary>
    public class StringAmountJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var amount = value as Amount;

            writer.WriteRawValue("\"" + amount.ToString(serializer.Culture) + "\"");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            var json = reader.Value.ToString();

            try
            {
                return Amount.Parse(json, serializer.Culture);
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to deserialize " + json + " at " + reader.Path + ".", ex);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Amount);
        }
    }
}
