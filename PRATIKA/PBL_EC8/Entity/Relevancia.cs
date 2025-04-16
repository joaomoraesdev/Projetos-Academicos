using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Relevancia{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("IdPost")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdPost { get; set; }

    [BsonElement("IdUsuario")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUsuario { get; set; }
}