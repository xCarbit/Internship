using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using MyGrpcService;
using MyGrpcService.Models;
using MyGrpcService.Services;

public class GreeterService : MyService.MyServiceBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly MongoService _mongoService;

    public GreeterService(ILogger<GreeterService> logger, MongoService mongoService)
    {
        _logger = logger;
        _mongoService = mongoService;
    }
    string Message = " ";

    public override Task<MyResponse> SendData(MyRequest request, ServerCallContext context)
    {
        Console.WriteLine($"Received: id={request.SensorId}, score={request.Value}");

        var data = new Data
        {
            SensorId = request.SensorId,
            SensorKind = request.SensorKind,
            Value = (decimal)request.Value,
            TimeStamp = DateTime.Parse(request.Time, null, System.Globalization.DateTimeStyles.RoundtripKind)
        };

        _mongoService.InsertData(data);
        Console.WriteLine("Podaci smesteni u bazu!");
          
        var response = new MyResponse 
        {
           Message=" Podaci validni!"
        };
        
        return Task.FromResult(response);
    }
}


