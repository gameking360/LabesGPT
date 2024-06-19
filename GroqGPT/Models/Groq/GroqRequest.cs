using GroqGPT.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GroqGPT.Models.Groq
{
    internal class GroqRequest
    {
        //Propriedade

        [JsonPropertyName("model")]
        public string Model { get; init; }

        [JsonPropertyName("temperature")]
        public double? Temperature { get; init; }

        [JsonPropertyName("messages")]
        public MessageModel[] Message { get; init; }

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; init; }

        [JsonPropertyName("top_p")]
        public double? TopP { get; init; }

        [JsonPropertyName("stop")]
        public string? Stop { get; init; }

        [JsonPropertyName("stream")]
        public bool Stream { get; init; }

        [JsonIgnore]
        public bool JsonResponse { get; set; } = false;


        public string ToJson()
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = false,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) } };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteStartObject();

                writer.WriteString("model", Model);
                if (Temperature.HasValue)
                    writer.WriteNumber("temperature", Temperature.Value);
                if (MaxTokens.HasValue)
                    writer.WriteNumber("max_tokens", MaxTokens.Value);
                if (TopP.HasValue)
                    writer.WriteNumber("top_p", TopP.Value);
                if (!string.IsNullOrEmpty(Stop))
                    writer.WriteString("stop", Stop);
                writer.WriteBoolean("stream", Stream);


                if (Message != null)
                {
                    writer.WritePropertyName("messages");
                    writer.WriteStartArray();
                    foreach(var msg in Message)
                    {
                        JsonSerializer.Serialize(writer, msg, msg.GetType(), options);

                    }

                    writer.WriteEndArray();
                }

                if (JsonResponse)
                {
                    writer.WritePropertyName("response_format");
                    writer.WriteStartObject();
                    writer.WriteString("type", "json_object");
                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
                writer.Flush();


            }

          

            return Encoding.UTF8.GetString(stream.ToArray());

        }
    }
}
