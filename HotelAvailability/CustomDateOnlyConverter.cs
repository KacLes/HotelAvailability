using System.Text.Json;
using System.Text.Json.Serialization;

namespace HotelAvailability
{
    public class CustomDateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString() ?? string.Empty, Constants.DATE_FORMAT, null);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Constants.DATE_FORMAT));
        }
    }

}
// See https://aka.ms/new-console-template for more information

