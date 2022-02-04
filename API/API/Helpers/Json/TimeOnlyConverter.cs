using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Helpers.Json;

#nullable enable
public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string _serializationFormat;

    public TimeOnlyConverter() : this(null) { }
    public TimeOnlyConverter(string? serializationFormat) =>
        _serializationFormat = serializationFormat ?? "HH:mm:ss.fff";


    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        TimeOnly.Parse((reader.GetString())!);

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString(_serializationFormat));
}
#nullable disable