using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using MyGrpcService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5007", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure
            });
            while (true)
            {
                //var client = new Greeter.GreeterClient(channel);
                var client = new MyService.MyServiceClient(channel);

                Console.Write("Unesi ime: ");
                var name = Console.ReadLine();
                Console.Write("Unesi Score: ");
                var score = Console.ReadLine();
                Console.Write("Unesi Count: ");
                var count = Console.ReadLine();
                var request = new MyRequest
                {
                    Name = name,
                    Score = float.Parse(score),
                    Count = int.Parse(count)
                };

                var reply = await client.SendDataAsync(request);


                Console.WriteLine("Podaci su uspesno poslati na server");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Greška pri komunikaciji sa serverom:");
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Pritisni Enter za izlaz...");
        Console.ReadLine();
    }
}