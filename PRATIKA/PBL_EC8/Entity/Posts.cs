using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Posts
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("Descricao")]
    public string Descricao { get; set; }

    [BsonElement("IdUsuario")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUsuario { get; set; }

    [BsonElement("QtdImpulsionamentos")]
    public string QtdImpulsionamentos { get; set; }

    [BsonElement("QtdCurtidas")]
    public string QtdCurtidas { get; set; }

    [BsonElement("FotoAnexo")]
    public string FotoAnexo { get; set; }

}
