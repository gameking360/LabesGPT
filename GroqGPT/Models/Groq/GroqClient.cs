using GroqGPT.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GroqGPT.Models.Groq
{
    internal class GroqClient
    {
        private const string _url = "https://api.groq.com/openai/v1/chat/completions";
        private const string ContentTypeJson = "application/json";
        private const string BearerToken = "Bearer";


        private readonly HttpClient httpClient = new HttpClient();



        private string Model;
        private double? Temperature;
        private int? TopP;
        private int MaxToxens;
        private bool Stream;
        private string Stop;


        public GroqClient(string model, HttpClient? client = null)
        {
          
            Model = model;

            httpClient = client ?? new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(BearerToken, Environment.GetEnvironmentVariable("GROQ_API_KEY"));
        }


        public GroqClient SetTemperature(double? temperature)
        {
            Temperature = (temperature == null || (temperature >= 0.0 && temperature <= 2.0)) ? temperature : throw new ArgumentOutOfRangeException("Temperature is a value between 0 and 2");
            return this;
        }

        public GroqClient SetMaxTokens(int maxTokens)
        {
            MaxToxens = maxTokens;
            return this;
        }

        public GroqClient SetTopP(int? topP)
        {
            TopP = topP;
            return this;
        }

        public GroqClient SetStop(string stop)
        {
            Stop = stop;
            return this;
        }

        public async Task<GroqResponse> SendMessage(params MessageModel[] mensagem)
        {
            if(mensagem == null || mensagem.Length == 0)
            {
                throw new ArgumentException("Mensagem can't be null or empty");

            }

           GroqRequest requests = new GroqRequest

              {
                    MaxTokens = this.MaxToxens,
                    Model = this.Model,
                    Stop = this.Stop,
                    TopP = this.TopP,
                    Temperature = this.Temperature,
                    Message = mensagem
                };
            

            

            try
            {
                
                GroqResponse responses = new GroqResponse();


                {
                    string requestJSON = requests.ToJson();
                    var httpContent = new StringContent(requestJSON, Encoding.UTF8, ContentTypeJson);
                    HttpResponseMessage response = await httpClient.PostAsync(_url, httpContent);
                    
                    var chatResponse = GroqResponse.ConvertFromJSON(await response.Content.ReadAsStringAsync());

                    return chatResponse;
                }
            }catch(Exception ex)
            {
                throw new Exception("Failed to create chat: " + ex.Message);
            }
        }

    }
}
