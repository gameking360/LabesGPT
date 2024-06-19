using GroqGPT.Models.Groq;
using GroqGPT.Models.Message;
using System;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using (var cts = new CancellationTokenSource())
        {

            Thread checar = new Thread(() => CheckEndProgram(cts));

            checar.Start();

                            

                GroqClient client = new GroqClient("llama3-70b-8192")
                    .SetTemperature(0.5)
                    .SetMaxTokens(256)
                    .SetTopP(1)
                    .SetStop("NONE");


                while (!cts.Token.IsCancellationRequested)
                {
                    Console.WriteLine("Sua mensagem");

                    string mensagem = await ConsoleReadAsync(cts);

                    var response = await client.SendMessage(new MessageModel { Role = MessageRole.user, Content = mensagem });

                    foreach (var msg in response.Contents)
                    {
                        Console.WriteLine("Resposta do bot:");
                        Console.WriteLine(msg);
                    }
                    await Task.Delay(100);
                }
            }
       }

    private static void CheckEndProgram(CancellationTokenSource cts)
    {
        while (!cts.Token.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept : true);
                if(key.Key == ConsoleKey.Escape)
                {
                   Console.WriteLine("Programa cancelado.");
                   Environment.Exit(10);
                }
            }
        }
    }

    private static async Task<string> ConsoleReadAsync(CancellationTokenSource cts)
    {
        var input = new System.Text.StringBuilder();

        while (!cts.Token.IsCancellationRequested)
        {

            if (Console.KeyAvailable)
            {


                var key = Console.ReadKey(intercept: true);


                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input.ToString();
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

        return null;
    }
}