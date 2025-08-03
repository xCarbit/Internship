namespace MyGrpcService.Models
{
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Data
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("SensorId")]
    public int SensorId { get; set; }

    [BsonElement("SensorKind")]
    public string SensorKind { get; set; }
 
    [BsonElement("Value")]
    public decimal Value { get; set; }

    [BsonElement("TimeStamp")]
    public DateTime TimeStamp { get; set; }

}
}