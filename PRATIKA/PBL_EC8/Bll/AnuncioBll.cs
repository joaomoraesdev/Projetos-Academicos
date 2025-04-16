using System.Security;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Routing.Template;

namespace PBL_EC8.Bll;

public class AnuncioBll : IAnuncioBll
{
    private readonly IMongoCollection<Anuncio> anunciosCollection;

    // Construtor que injeta a dependência de IMongoClient
    public AnuncioBll(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        anunciosCollection = database.GetCollection<Anuncio>(collectionName);
    }

    public async Task<RetornoAcaoDto> CriarAnuncio(AnuncioDto anuncioDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            Anuncio anuncio = new Anuncio();
            FilterDefinition<Anuncio> filtro = Builders<Anuncio>.Filter.Eq(m => m.IdUsuario, anuncioDto.IdUsuario);
            anuncio = await anunciosCollection.Find(filtro).FirstOrDefaultAsync();
            if (anuncio == null)
            {
                anuncio = ConverterAnuncioDto(anuncioDto);
                await anunciosCollection.InsertOneAsync(anuncio);
                retorno.Mensagem = "Anuncio inserido com sucesso!";
                retorno.Sucesso = true;
                return retorno;
            }
            else
            {
                retorno.Mensagem = "Já existe um anúncio para o seu perfil!";
                retorno.Sucesso = false;
            }
        }
        catch (Exception ex)
        {
            retorno.Mensagem = "Falha ao criar o anúncio...";
            retorno.Sucesso = false;
        }
        return retorno;
    }

    public async Task<List<Anuncio>> PesquisarAnunciosPorUsuario(string IdUsuario)
    {
        var filtro = Builders<Anuncio>.Filter.Eq(anuncio => anuncio.IdUsuario, IdUsuario);

        var anuncios = await anunciosCollection.Find(filtro).ToListAsync();

        if (anuncios == null || anuncios.Count == 0)
            return null;

        return anuncios;
    }

    public async Task<List<AnuncioDto>> PesquisarTodosAnuncios()
    {
        List<AnuncioDto> retorno = new List<AnuncioDto>();
        List<Anuncio> listaAnuncio = await anunciosCollection.Find(_ => true).ToListAsync();
        foreach (Anuncio anuncio in listaAnuncio)
            retorno.Add(ConverterAnuncio(anuncio));
        return retorno;
    }

    public async Task<List<AnuncioDto>> PesquisarAnuncios(string pesquisa)
    {
        List<AnuncioDto> retorno = new List<AnuncioDto>();
        var filtro = Builders<Anuncio>.Filter.Or
                        (
                            Builders<Anuncio>.Filter.Regex(u => u.NomeAnunciante, new MongoDB.Bson.BsonRegularExpression(pesquisa, "i")),
                            Builders<Anuncio>.Filter.Regex(u => u.Profissao, new MongoDB.Bson.BsonRegularExpression(pesquisa, "i")),
                            Builders<Anuncio>.Filter.Regex(u => u.Cidade, new MongoDB.Bson.BsonRegularExpression(pesquisa, "i")),
                            Builders<Anuncio>.Filter.Regex(u => u.Estado, new MongoDB.Bson.BsonRegularExpression(pesquisa, "i")),
                            Builders<Anuncio>.Filter.Regex(u => u.Descricao, new MongoDB.Bson.BsonRegularExpression(pesquisa, "i"))
                        );
        List<Anuncio> listaAnuncio = await anunciosCollection.Find(filtro).ToListAsync();
        foreach (Anuncio anuncio in listaAnuncio)
            retorno.Add(ConverterAnuncio(anuncio));
        return retorno;
    }

    public async Task<RetornoAcaoDto> ExcluirAnuncio(AnuncioDto anuncioDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            var filter = Builders<Anuncio>.Filter.Eq(p => p.Id, anuncioDto.Id);
            
            await anunciosCollection.DeleteOneAsync(filter);
            retorno.Mensagem = "Anúncio excluido com sucesso!";
            retorno.Sucesso = true;
        }
        catch (Exception ex)
        {
            retorno.Mensagem = $"Falha ao excluir o anúncio: {ex.Message}";
            retorno.Sucesso = false;
        }
        return retorno;
    }

    public async Task<RetornoAcaoDto> EditarAnuncio(AnuncioDto anunciosDto)
{
    RetornoAcaoDto retorno = new RetornoAcaoDto();
    try
    {
        // Filtro para localizar o anúncio pelo ID
        var filter = Builders<Anuncio>.Filter.Eq(a => a.Id, anunciosDto.Id);
       
        // Atualização dos campos do anúncio
        var update = Builders<Anuncio>.Update
            .Set(a => a.Titulo, anunciosDto.Titulo) // Atualiza o título
            .Set(a => a.Profissao, anunciosDto.Profissao) // Atualiza a profissão
            .Set(a => a.Estado, anunciosDto.Estado) // Atualiza o estado
            .Set(a => a.Cidade, anunciosDto.Cidade) // Atualiza a cidade
            .Set(a => a.Descricao, anunciosDto.Descricao); // Atualiza a descrição

        // Executa a atualização no MongoDB
        var result = await anunciosCollection.UpdateOneAsync(filter, update);
            
        retorno.Mensagem = "Anúncio editado com sucesso!";
        retorno.Sucesso = true;
    }
    catch (Exception ex)
    {
        retorno.Mensagem = $"Falha ao editar o anúncio: {ex.Message}";
        retorno.Sucesso = false;
    }
    return retorno;
}


    private Anuncio ConverterAnuncioDto(AnuncioDto anuncioDto)
    {
        Anuncio anuncio = new Anuncio();

        anuncio.Titulo = anuncioDto.Titulo;
        anuncio.IdUsuario = anuncioDto.IdUsuario;
        anuncio.Profissao = anuncioDto.Profissao;
        anuncio.Estado = anuncioDto.Estado;
        anuncio.Cidade = anuncioDto.Cidade;
        anuncio.Descricao = anuncioDto.Descricao;
        anuncio.NomeAnunciante = anuncioDto.NomeAnunciante;
        anuncio.ImagemAnunciante = anuncioDto.ImagemAnunciante;
        return anuncio;
    }

    private AnuncioDto ConverterAnuncio(Anuncio anuncio)
    {
        AnuncioDto anuncioDto = new AnuncioDto();

        anuncioDto.Id = anuncio.Id;
        anuncioDto.Titulo = anuncio.Titulo;
        anuncioDto.IdUsuario = anuncio.IdUsuario;
        anuncioDto.Profissao = anuncio.Profissao;
        anuncioDto.Estado = anuncio.Estado;
        anuncioDto.Cidade = anuncio.Cidade;
        anuncioDto.Descricao = anuncio.Descricao;
        anuncioDto.NomeAnunciante = anuncio.NomeAnunciante;
        anuncioDto.ImagemAnunciante = anuncio.ImagemAnunciante;
        return anuncioDto;
    }
}
