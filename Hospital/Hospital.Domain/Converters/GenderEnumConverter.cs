using Hospital.Domain.Values;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hospital.Domain.Converters
{
    public class GenderEnumConverter: JsonConverter<Gender>
    {
        public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (Gender)Enum.Parse(typeof(Gender), reader.GetString(), true);
        }

        public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
