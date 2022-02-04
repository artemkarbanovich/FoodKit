using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Helpers.Json;

#nullable enable
public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private readonly string _serializationFormat;

    public DateOnlyConverter() : this(null) { }
    public DateOnlyConverter(string? serializationFormat) =>
        _serializationFormat = serializationFormat ?? "yyyy-MM-dd";


    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateOnly.Parse((reader.GetString())!);

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(_serializationFormat));
}
#nullable disable