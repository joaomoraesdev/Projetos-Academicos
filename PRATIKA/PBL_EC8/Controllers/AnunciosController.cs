using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pbl_EC8.Models;
using PBL_EC8.Bll;
using PBL_EC8;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;

namespace PBL_EC8.Controllers;

public class AnunciosController : Controller
{
    private readonly ILogger<AnunciosController> _logger;
    private readonly UsuarioBll usuarioBll;

    private readonly AnuncioBll anuncioBll;

    // Construtor injetando tanto o ILogger quanto o UsuarioBll
    public AnunciosController(ILogger<AnunciosController> logger, UsuarioBll _usuarioBll, AnuncioBll _anuncioBll)
    {
        _logger = logger;
        usuarioBll = _usuarioBll;
        anuncioBll = _anuncioBll;
    }

    // [Authorize]
    public IActionResult AnunciosIndex()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> ValidacaoPerfil()
    {
        UsuarioDto usuarioDto = new UsuarioDto();

        if (HttpContext.Session.GetString("Usuario") == null)
            return Json(new { success = false, message = "Falha ao recuperar o Nome de Usuário!" });
        else
            usuarioDto.NomeUsuario = HttpContext.Session.GetString("Usuario");

        UsuarioDto retornoUsuario = await usuarioBll.PesquisarUsuario(usuarioDto);
        RetornoAcaoDto retorno = new RetornoAcaoDto();

        if (retornoUsuario.Perfil == "Premium")
        {
            retorno.Sucesso = true;
            retorno.Mensagem = "Perfil Premium identificado";
        }
        else
        {
            retorno.Sucesso = false;
            retorno.Mensagem = "Perfil Premium não identificado";
        }

        return Json(new { success = retorno.Sucesso, message = retorno.Mensagem, imagemPerfil = retornoUsuario.ImagemPerfil });
    }

    [HttpPost]
    public async Task<JsonResult> CadastrarAnuncio(AnuncioDto dto)
    {
       
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        UsuarioDto usuarioDto = new UsuarioDto();
        usuarioDto.NomeUsuario = HttpContext.Session.GetString("Usuario");
        usuarioDto = await usuarioBll.PesquisarUsuario(usuarioDto);
        if (usuarioDto.Id != null)
        {
            if(dto.Id != null){
                retorno = await anuncioBll.EditarAnuncio(dto);
                return Json(new { success = retorno.Sucesso, message = retorno.Mensagem });
            }else{
                dto.IdUsuario = usuarioDto.Id;
                dto.NomeAnunciante = usuarioDto.Nome;
                retorno = await anuncioBll.CriarAnuncio(dto);
                return Json(new { success = retorno.Sucesso, message = retorno.Mensagem });
            }
        }
        else
            return Json(new { success = false, message = "Falha em identificar o usuário do anúncio!" });
    }

    [HttpPost]
    public async Task<JsonResult> PesquisarTodosAnuncios()
    {
        List<AnuncioDto> listaAnuncio = new List<AnuncioDto>();
        listaAnuncio = await anuncioBll.PesquisarTodosAnuncios();
        return Json(new { success = true, lista = listaAnuncio });
    }

    [HttpPost]
    public async Task<JsonResult> PesquisarAnunciosPorUsuario()
    {
        UsuarioDto usuarioDto = new UsuarioDto();
        usuarioDto.NomeUsuario = HttpContext.Session.GetString("Usuario");
        usuarioDto = await usuarioBll.PesquisarUsuario(usuarioDto);

        List<Anuncio> listaAnuncio = new List<Anuncio>();
        listaAnuncio = await anuncioBll.PesquisarAnunciosPorUsuario(usuarioDto.Id);
        return Json(new { success = true, lista = listaAnuncio });
    }

    [HttpPost]
    public async Task<JsonResult> PesquisarAnuncios(string pesquisa)
    {
        List<AnuncioDto> listaAnuncio = new List<AnuncioDto>();
        listaAnuncio = await anuncioBll.PesquisarAnuncios(pesquisa);
        return Json(new { success = true, lista = listaAnuncio });
    }

    [HttpPost]
    public async Task<JsonResult> ExcluirAnuncio(AnuncioDto anuncio)
    {
        try
        {
            var retorno = await anuncioBll.ExcluirAnuncio(anuncio);

            return Json(new { success = retorno.Sucesso, message = retorno.Mensagem });
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao excluir anuncio: {Message}", ex.Message);
            return Json(new { success = false, message = "Erro ao excluir anuncio." });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
