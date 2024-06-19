using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GroqGPT.Models.Message
{
    internal class MessageModel
    {
        public MessageRole Role { get; set; }
        public string Content { get; set; }

        public virtual string ToJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)}
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}
