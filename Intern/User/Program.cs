using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using MyGrpcService;
using System.Globalization;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Net.Http;
class Program
{
     static Grpc.Net.Client.GrpcChannel channel;
     static  async Task Main()
    {
            try
        {
             channel = GrpcChannel.ForAddress("http://localhost:5007", new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure
            });
       
        
        
        var client = new MyService.MyServiceClient(channel);
        

        Console.WriteLine("Odaberite jednu od opcija: ");
       Console.WriteLine("1. Prikaz svih ocitavanja sa odredjenog senzora u odredjenom periodu");
       Console.WriteLine("2. Prikaz prosečne, minimalne i maksimalne vrednosti za određeni tip senzora u nekom vremenskom intervalu");
       Console.WriteLine("3. Prikaz svih očitavanja većih od neke zadate vrednosti za određeni tip senzora u nekom vremenskom intervalu");
      while(true){
       Console.WriteLine();
       Console.WriteLine("Unesite redni broj opcije: ");
       int op=int.Parse(Console.ReadLine());


       Console.WriteLine("Unesite id zeljenog senzora: ");
          int senId=int.Parse(Console.ReadLine());
          Console.WriteLine("Unesite datum donje granice intervala (u obliku. YYYY-MM-DD):");
          String s1=Console.ReadLine();
          Console.WriteLine("Unesite vreme donje granice intervala (u obliku HH:MM):");
          String s2=Console.ReadLine();
          String vremeStr1=s1+"T"+s2+":00.00+00:00";
          //DateTimeOffset vremeDonje = DateTimeOffset.Parse(vremeStr1);

         Console.WriteLine("Unesite datum gornje granice intervala (u obliku. YYYY-MM-DD):");
           s1=Console.ReadLine();
          Console.WriteLine("Unesite vreme gornje granice intervala (u obliku HH:MM):");
           s2=Console.ReadLine();
          String vremeStr2=s1+"T"+s2+":00.00+00:00";
         // DateTimeOffset vremeGornje = DateTimeOffset.Parse(vremeStr2);
           
       if(op==1)
       {
           var request = new MyRequest { SensorId = senId, TimeFrom = vremeStr1, TimeTo= vremeStr2 };
           var call = await  client.GetDataIdParticularPeriodAsync(request);
      

        foreach (var data in call.Data)
        {
               Console.WriteLine($"SensorId: {data.SensorId}, TimeStamp: {data.TimeStamp}, Value: {data.Value}");
        }
         

       }
       else if(op==2)
       {
       
        double minimum=double.MaxValue;
         double maximum=double.MinValue;
         double average=0;
         double cnt=0;
           string s="";

         var request = new MyRequest { SensorId = senId, TimeFrom = vremeStr1, TimeTo= vremeStr2 };
           var call = await  client.GetDataIdParticularPeriodAsync(request);
      

        foreach (var o in call.Data)
        {
            if(o.SensorKind=="Temperatura [°C]") s= "°C";
                else s="mbar";
                minimum=minimum<= o.Value ? minimum : o.Value;
                maximum=maximum >=o.Value ? maximum : o.Value;
                average=average+o.Value;
                cnt=cnt+1;
        }

        if(cnt>0)
        {
            average=average/cnt;
            
            if(s=="°C") {
                Console.WriteLine("Minimalna izmerena temperatura je " + minimum + " " +s);
                Console.WriteLine("Maksimalna izmerena temperatura je " + maximum + " " +s);
                Console.WriteLine("Srednja izmerena temperatura je " + average + " " +s);
            }
            else {
                Console.WriteLine("Minimalni izmereni pritisak je " + minimum + " " +s);
                Console.WriteLine("Maksimalni izmereni pritisak je " + maximum + " " +s);
                Console.WriteLine("Srednji izmereni pritisak je " + average + " " +s);
            }
        }
         else {
            Console.WriteLine("U bazi ne postoje merenja koja zadovoljavjau postalvjene kriterijme!");
         }
    
       }
       else if (op==3)
       {
         

           string st="";
             Console.WriteLine("Unesite referentnu vrednost: ");
           double refValue=double.Parse(Console.ReadLine());
           var request = new MyRequest1 { SensorId = senId, TimeFrom = vremeStr1, TimeTo= vremeStr2, RefValue=refValue };
           var call = await  client.GetDataIdParticularPeriodRefValueAsync(request);
      

        foreach (var o in call.Data)
        {
             if(o.SensorId==senId)
                {
                    if(o.SensorKind=="Temperatura [°C]")st= "[°C]";
                    else st="[mbar]";
                    break;
                }
        }

          foreach (Data o in call.Data)
         {
            
                Console.WriteLine("Senzor sa Id "+ o.SensorId + " je izmerio " + o.Value + " " + st + " u trenutku " + o.TimeStamp + ".");
         }
          
        } 
       }
      
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

}