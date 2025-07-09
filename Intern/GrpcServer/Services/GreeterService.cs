using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using MyGrpcService;


public class GreeterService : MyService.MyServiceBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<Empty> SendData(MyRequest request, ServerCallContext context)
    {
        Console.WriteLine($"Received: name={request.Name}, score={request.Score}, count={request.Count}");
        return Task.FromResult(new Empty());
    }
}
