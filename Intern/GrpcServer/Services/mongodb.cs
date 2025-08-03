using MongoDB.Driver;
using MyGrpcService.Models;

namespace MyGrpcService.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<Data> _collection;

        public MongoService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
            _collection = database.GetCollection<Data>("MyData");
        }

        public void InsertData(Data data)
        {
            _collection.InsertOne(data);
        }
    }
}