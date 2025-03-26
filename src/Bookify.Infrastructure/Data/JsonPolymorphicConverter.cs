using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bookify.Infrastructure.Data;

// Alernative for Newtonsoft.Json TypeNameHandling
public class JsonPolymorphicConverter<T> : JsonConverter<T>
{
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
            return;
        var type = value.GetType();
        var typeInfo = $"{type.FullName}, {type.Assembly.GetName().Name}";

        var json = JsonSerializer.Serialize(value, value.GetType(), options);
        using var doc = JsonDocument.Parse(json);
        writer.WriteStartObject();
        writer.WriteString("$type", typeInfo);
        var properties = doc.RootElement.EnumerateObject();
        foreach (var prop in properties)
        {
            prop.WriteTo(writer);
        }
        writer.WriteEndObject();
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        var typeName = root.GetProperty("$type").GetString() ?? throw new JsonException("Missing $type property.");
        var type = Type.GetType(typeName) ?? throw new JsonException("Invalid type name.");
        
        var mutableJson = new Dictionary<string, JsonElement>();
        var properties = root.EnumerateObject().Where(prop => prop.Name != "$type");
        foreach (var prop in properties)
        {
            mutableJson[prop.Name] = prop.Value;
        }
        
        var json = JsonSerializer.Serialize(mutableJson);
        return (T)JsonSerializer.Deserialize(json, type, options)!;
    }

}