using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Shared.Utils
{
    public static class JsonConverters
    {
        public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
        {
            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType is JsonTokenType.Number)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64());
                }
                else { return default; }
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value.ToUnixTimeSeconds());
            }
        }
    }
}
