using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using MyGrpcService;
using System.Globalization;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
class Program
{
    static System.Timers.Timer timer;
    static int maxVal; static int minVal;
    static Random random = new Random();
    static int sensorId;
    static string tipSenzora;
    static Grpc.Net.Client.GrpcChannel channel;
   
    
    static async Task Main(string[] args)
    {
        try
        {
             channel = GrpcChannel.ForAddress("http://localhost:5007", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure
            });

           
            while (true)
            {
               Console.Write("Unesite tip senzora (TEMPERATURA | PRITISAK): ");

              
                tipSenzora = Console.ReadLine();
                if(tipSenzora!="TEMPERATURA" && tipSenzora!="PRITISAK")
                {
                    Console.Write("NEPOSTOJECI TIP SENZORA!");
                   return;
                }

                Console.Write("Unesite id sensora: ");
                sensorId=int.Parse(Console.ReadLine());

               if(tipSenzora=="TEMPERATURA") {
                Console.Write("Unesite minumalnu vrednost koju senzor moze ocitati (u °C): ");
                minVal = int.Parse(Console.ReadLine());
                Console.Write("Unesite maksimalnu vrednost koju senzor moze ocitati (u °C): ");
                maxVal = int.Parse(Console.ReadLine());
                tipSenzora="Temperatura [°C]";
            }
             else {
                Console.Write("Unesite minumalnu vrednost koju senzor moze ocitati (u mbar): ");
                minVal = int.Parse(Console.ReadLine());
                Console.Write("Unesite maksimalnu vrednost koju senzor moze ocitati (u mbar): ");
                maxVal = int.Parse(Console.ReadLine());
                tipSenzora="Pritisak [mbar]";
             }
                Console.Write("Unesite vremenski interval izmedju dva timestampa (u milisekundama): ");
                int timeStampDistance = int.Parse(Console.ReadLine());
               

               timer = new System.Timers.Timer(timeStampDistance); 
               timer.Elapsed += OnTimedEvent;
               timer.AutoReset = true;
               timer.Enabled = true;

               Console.WriteLine("Press [Enter] to exit.");
               Console.ReadLine();

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

    static async void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        double val=0; 
        if(tipSenzora=="Temperatura [°C]") { 
            val=new Random().Next(minVal,maxVal);
        }
           
        else  {val=minVal + random.NextDouble()*(maxVal-minVal); val=Math.Round(val,2);}
        Console.WriteLine(val);
        DateTime originalTime = DateTime.Now;      
        DateTime adjustedTime = originalTime.AddHours(2);
        var request = new MyRequest
                {
                    SensorId = sensorId,
                    SensorKind = tipSenzora, 
                    Value = val,
                    Time = adjustedTime.ToString("o")

                };

                 var client = new MyService.MyServiceClient(channel);

               var response = await client.SendDataAsync(request);

               Console.WriteLine($"[KLIJENT] Odgovor servera: {response.Message}");

    }
}