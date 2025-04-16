using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Anuncio
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // Permite trabalhar com string em vez de ObjectId
    public string Id { get; set; } // Agora o Id pode ser tratado como string

    [BsonElement("Titulo")]
    public string Titulo { get; set; }

    [BsonElement("IdUsuario")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUsuario { get; set; }

    [BsonElement("NomeAnunciante")]
    public string NomeAnunciante { get; set; }

    [BsonElement("ImagemAnunciante")]
    public string ImagemAnunciante { get; set; }

    [BsonElement("Profissao")]
    public string Profissao { get; set; }

    [BsonElement("Estado")]
    public string Estado { get; set; }

    [BsonElement("Cidade")]
    public string Cidade { get; set; }

    [BsonElement("Descricao")]
    public string Descricao { get; set; }
}
