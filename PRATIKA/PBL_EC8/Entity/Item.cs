using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // Permite trabalhar com string em vez de ObjectId
    public string Id { get; set; } // Agora o Id pode ser tratado como string

    [BsonElement("Titulo")]
    public string Titulo { get; set; }

    [BsonElement("IdUsuario")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdUsuario { get; set; }

    [BsonElement("Descricao")]
    public string Descricao { get; set; }

    [BsonElement("Valor")]
    public string Valor { get; set; }

    [BsonElement("ImagemItem")]
    public string ImagemItem { get; set; }

    [BsonElement("Contato")]
    public string Contato { get; set; }

    [BsonElement("Quantidade")]
    public string Quantidade { get; set; }

    [BsonElement("MetodoPagamento")]
    public string MetodoPagamento { get; set; }
}
