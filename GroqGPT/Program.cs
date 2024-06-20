using GroqGPT.Models.Groq;
using GroqGPT.Models.Message;
using System;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {

        GroqClient client = new GroqClient("llama3-70b-8192")
                     .SetTemperature(0.5)
                     .SetMaxTokens(256)
                     .SetTopP(1)
                     .SetStop("NONE");


        while (true)
        {
            Console.Write("Sua mensagem: ");

            string mensagem = Console.ReadLine();

            var response = await client.SendMessage(new MessageModel { Role = MessageRole.user, Content = mensagem });

            foreach (var msg in response.Contents)
            {
                Console.WriteLine("\nResposta do bot:");
                Console.WriteLine(msg);
            }
            await Task.Delay(100);
        }

    }
}
