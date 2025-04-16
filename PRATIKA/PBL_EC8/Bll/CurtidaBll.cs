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

public class CurtidaBll : ICurtidaBll
{
    private readonly IMongoCollection<Curtida> curtidaCollection;

    public CurtidaBll(IMongoClient mongoClient, string databaseName, string collectionName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        curtidaCollection = database.GetCollection<Curtida>(collectionName);
    }

     public async Task<RetornoAcaoDto> CriarCurtida(CurtidaDto curtidaDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            Curtida curtida = ConverterCurtidaDto(curtidaDto);
           
            await curtidaCollection.InsertOneAsync(curtida);
            retorno.Mensagem = "Curtida inserido com sucesso!";
            retorno.Sucesso = true;
        }
        catch (Exception ex)
        {
            retorno.Mensagem = $"Falha ao criar curtida: {ex.Message}";
            retorno.Sucesso = false;
        }
        return retorno;
    }

     public async Task<RetornoAcaoDto> RetirarCurtida(CurtidaDto curtidaDto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        try
        {
            var filtro = Builders<Curtida>.Filter.Eq(c => c.Id, curtidaDto.Id);
           
            await curtidaCollection.DeleteOneAsync(filtro);
            retorno.Mensagem = "Curtida excluido com sucesso!";
            retorno.Sucesso = true;
        }
        catch (Exception ex)
        {
            retorno.Mensagem = $"Falha ao excluir curtida: {ex.Message}";
            retorno.Sucesso = false;
        }
        return retorno;
    }

    public async Task<List<CurtidaDto>> BuscarCurtidas()
    {
        List<CurtidaDto> retorno = new List<CurtidaDto>();
        List<Curtida> listaCurtidas = await curtidaCollection.Find(_ => true).ToListAsync();
        foreach(Curtida curtida in listaCurtidas){
            retorno.Add(ConverterCurtidaEntidade(curtida));
        }
            
        return retorno;
    }

    public Curtida ConverterCurtidaDto(CurtidaDto curtidaDto)
    {
        Curtida entidade = new Curtida();
       
        entidade.Id = curtidaDto.Id;
        entidade.IdUsuario = curtidaDto.IdUsuario;
        entidade.IdPost = curtidaDto.IdPost;
        
        return entidade;
    }

    public CurtidaDto ConverterCurtidaEntidade(Curtida entidade)
    {
        CurtidaDto dto = new CurtidaDto();
       
        dto.Id = entidade.Id;
        dto.IdUsuario = entidade.IdUsuario;
        dto.IdPost = entidade.IdPost;

        return dto;
    }

}