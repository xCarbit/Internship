using MongoDB.Driver;
using MyGrpcService.Models; 

public class MongoService
{
    private readonly IMongoCollection<Data> podaci;

    public MongoService()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("MyDatabase");
        podaci = database.GetCollection<Data>("MyData");
    }

   

    public List<Data> VratiSve()
    {
        return podaci.Find(_ => true).ToList();
    }
}