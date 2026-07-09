using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CustomDateConverter : JsonConverter<DateTime>
{
    private readonly string _format = "dd.MM.yyyy HH:mm";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString();
        return DateTime.ParseExact(dateString, _format, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}