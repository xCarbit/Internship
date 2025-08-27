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

    public override Task<MyResponse1> SendData(MyRequest1 request, ServerCallContext context)
    {
        Console.WriteLine($"Received: id={request.SensorId}, score={request.Value}");
       
        var data = new Data
        {
            SensorId = request.SensorId,
            SensorKind = request.SensorKind,
            Value = request.Value,
            TimeStamp = DateTime.Parse(request.Time, null, System.Globalization.DateTimeStyles.RoundtripKind)
        };

        _mongoService.InsertData(data);
        Console.WriteLine("Podaci smesteni u bazu!");
          
        var response = new MyResponse1 
        {
           Message=" Podaci validni!"
        };
        
        return Task.FromResult(response);
    }

   
    public override Task<MyResponse2> GetDataIdParticularPeriod(MyRequest2 request, ServerCallContext context)
    {
        var from = DateTime.Parse(request.FromTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        var to   = DateTime.Parse(request.ToTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        var list=_mongoService.GetDataIdParticularPeriod(request.SensorId,from,to);

        var response = new MyResponse2();
        response.Data.AddRange(list.Select(d => new MyData
       {
           Id = d.Id,
           SensorId = d.SensorId,
           TimeStamp = d.TimeStamp.ToString("o"),
           Value = d.Value
    }));

    
    return Task.FromResult(response);


    }

    public override Task<MyResponse2> GetDataIdParticularPeriodRefValue(MyRequestOne request, ServerCallContext context)
    {
        var from = DateTime.Parse(request.FromTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        var to   = DateTime.Parse(request.ToTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
        var list=_mongoService.GetDataIdParticularPeriodRefValue(request.SensorId,from,to, request.RefValue);

        var response = new MyResponse2();
        response.Data.AddRange(list.Select(d => new MyData
       {
           Id = d.Id,
           SensorId = d.SensorId,
           TimeStamp = d.TimeStamp.ToString("o"),
           Value = d.Value
    }));

    
    return Task.FromResult(response);


    }

    
}


