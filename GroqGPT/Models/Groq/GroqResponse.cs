using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GroqGPT.Models.Groq
{
    internal class GroqResponse
    {
        public List<string> Contents { get; internal set; } = new List<string>();


        public static GroqResponse ConvertFromJSON(string json)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    

                    if (!root.TryGetProperty("choices", out var choices))
                    {
                        throw new JsonException("'choices' is missing");
                    }

                    var response = new GroqResponse();
                    foreach (var choice in choices.EnumerateArray())
                    {
                        if(choice.TryGetProperty("message", out var message))
                        {
                            if (message.TryGetProperty("content", out var content) && content.GetString() != null)
                            {
                                response.Contents.Add(content.GetString());
                            }
                        }
                    }
                    return response;
                }

                 
            }catch(Exception ex)
            {
                throw new JsonException("Fail to parse to JSON: " + ex.Message);
            }
        }
    }
}
