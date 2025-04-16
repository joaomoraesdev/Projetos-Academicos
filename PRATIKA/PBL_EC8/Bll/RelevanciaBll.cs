using System.Security;
using Microsoft.AspNetCore.Mvc;
using Pbl_EC8.Models;
using PBL_EC8.Bll;
using PBL_EC8;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Routing.Template;

namespace PBL_EC8.Bll;

public class RelevanciaBll : IRelevanciaBll
{
    private readonly IMongoCollection<Relevancia> relevanciaCollection;

    public RelevanciaBll(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        relevanciaCollection = database.GetCollection<Relevancia>(collectionName);
    }

    public async Task<RetornoAcaoDto> CriarRelevancia(RelevanciaDto relevanciaDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            Relevancia relevancia = ConverterRelevanciaDto(relevanciaDto);
           
            await relevanciaCollection.InsertOneAsync(relevancia);
            retorno.Mensagem = "relevancia inserido com sucesso!";
            retorno.Sucesso = true;
        }
        catch (Exception ex)
        {
            retorno.Mensagem = $"Falha ao criar relevancia: {ex.Message}";
            retorno.Sucesso = false;
        }
        return retorno;
    }

     public async Task<RetornoAcaoDto> RetirarRelevancia(RelevanciaDto relevanciaDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            var filtro = Builders<Relevancia>.Filter.Eq(c => c.Id, relevanciaDto.Id);
           
            await relevanciaCollection.DeleteOneAsync(filtro);
            retorno.Mensagem = "relevancia excluido com sucesso!";
            retorno.Sucesso = true;
        }
        catch (Exception ex)
        {
            retorno.Mensagem = $"Falha ao excluir relevancia: {ex.Message}";
            retorno.Sucesso = false;
        }
        return retorno;
    }

    public async Task<List<RelevanciaDto>> BuscarRelevancia()
    {
        List<RelevanciaDto> retorno = new List<RelevanciaDto>();
        List<Relevancia> listaRelevancia = await relevanciaCollection.Find(_ => true).ToListAsync();
        
        foreach(Relevancia relevancia in listaRelevancia){
            retorno.Add(ConverterRelevanciaEntidade(relevancia));
        }
        
        return retorno;
    }

    public Relevancia ConverterRelevanciaDto(RelevanciaDto relevanciaDto)
    {
        Relevancia entidade = new Relevancia();
       
        entidade.Id = relevanciaDto.Id;
        entidade.IdUsuario = relevanciaDto.IdUsuario;
        entidade.IdPost = relevanciaDto.IdPost;
        
        return entidade;
    }

    public RelevanciaDto ConverterRelevanciaEntidade(Relevancia entidade)
    {
        RelevanciaDto dto = new RelevanciaDto();
       
        dto.Id = entidade.Id;
        dto.IdUsuario = entidade.IdUsuario;
        dto.IdPost = entidade.IdPost;

        return dto;
    }

}