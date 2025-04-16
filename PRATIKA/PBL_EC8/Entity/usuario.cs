using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Usuario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // Permite trabalhar com string em vez de ObjectId
    public string Id { get; set; } // Agora o Id pode ser tratado como string

    [BsonElement("NomeUsuario")]
    public string NomeUsuario { get; set; }

    [BsonElement("ImagemPerfil")]
    public string ImagemPerfil { get; set; }

    [BsonElement("Nome")]
    public string Nome { get; set; }

    [BsonElement("Sobrenome")]
    public string Sobrenome { get; set; }

    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("Senha")]
    public string Senha { get; set; }

    [BsonElement("DataNascimento")]
    public DateTime DataNascimento { get; set; }

    [BsonElement("Descricao")]
    public string Descricao { get; set; }

    [BsonElement("Genero")]
    public string Genero { get; set; }

    [BsonElement("Perfil")]
    public string Perfil { get; set; }
}
