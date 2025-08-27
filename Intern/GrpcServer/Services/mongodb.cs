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
        public List<Data> getDataId(int id)
        {
            var filter = Builders<Data>.Filter.Eq(s => s.SensorId, id);
            return _collection.Find(filter).ToList();
        }
    
    public  List<Data> GetDataIdParticularPeriod(int id, DateTime from, DateTime to)
    {

      var filter = Builders<Data>.Filter.And(
        Builders<Data>.Filter.Eq(d => d.SensorId, id),
        Builders<Data>.Filter.Gte(d => d.TimeStamp, from),
        Builders<Data>.Filter.Lte(d => d.TimeStamp, to)
    );

      return  _collection.Find(filter).ToList();

    }
      
     public  List<Data> GetDataIdParticularPeriodRefValue(int id, DateTime from, DateTime to, double refValue)
    {

      var filter = Builders<Data>.Filter.And(
        Builders<Data>.Filter.Eq(d => d.SensorId, id),
        Builders<Data>.Filter.Gte(d => d.TimeStamp, from),
        Builders<Data>.Filter.Lte(d => d.TimeStamp, to), 
        Builders<Data>.Filter.Gte(d => d.Value, refValue)
    );

      return  _collection.Find(filter).ToList();

    }


    }
}