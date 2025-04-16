using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pbl_EC8.Models;
using PBL_EC8.Bll;
using PBL_EC8;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;

namespace PBL_EC8.Controllers;

public class MarketplaceController : Controller
{
    private readonly ILogger<MarketplaceController> _logger;
    private readonly UsuarioBll usuarioBll;

    private readonly MarketplaceBll marketplaceBll;

    // Construtor injetando tanto o ILogger quanto o UsuarioBll
    public MarketplaceController(ILogger<MarketplaceController> logger, UsuarioBll _usuarioBll, MarketplaceBll _marketplaceBll)
    {
        _logger = logger;
        usuarioBll = _usuarioBll;
        marketplaceBll = _marketplaceBll;
    }

    // [Authorize]
    public IActionResult MarketplaceIndex()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> CadastrarItem(ItemDto dto)
    {
        RetornoAcaoDto retorno = new RetornoAcaoDto();
        UsuarioDto usuarioDto = new UsuarioDto();
        usuarioDto.NomeUsuario = HttpContext.Session.GetString("Usuario");
        usuarioDto = await usuarioBll.PesquisarUsuario(usuarioDto);
        if (usuarioDto.Id != null)
        {
            if(dto.Id != null){
                retorno = await marketplaceBll.EditarItem(dto);
                return Json(new { success = retorno.Sucesso, message = retorno.Mensagem });
            }else{
                dto.IdUsuario = usuarioDto.Id;
                retorno = await marketplaceBll.CriarItem(dto);
                return Json(new { success = retorno.Sucesso, message = retorno.Mensagem });
            }
           
        }
        else
            return Json(new { success = false, message = "Falha em identificar o usuário do anúncio!" });
    }

    [HttpPost]
    public async Task<JsonResult> PesquisarTodosItens()
    {
        List<ItemDto> listaItem = new List<ItemDto>();
        listaItem = await marketplaceBll.PesquisarTodosItens();
        listaItem = await PreencheUsuario(listaItem);
        return Json(new { success = true, lista = listaItem });
    }

    [HttpPost]
    public async Task<JsonResult> PesquisarItens(string pesquisa)
    {
        List<ItemDto> listaItem = new List<ItemDto>();
        listaItem = await marketplaceBll.PesquisarItens(pesquisa);
        listaItem = await PreencheUsuario(listaItem);
        return Json(new { success = true, lista = listaItem });
    }

    private async Task<List<ItemDto>> PreencheUsuario(List<ItemDto> listaItem)
    {
        foreach (ItemDto item in listaItem)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario.Id = item.IdUsuario;
            try
            {
                usuario = await usuarioBll.PesquisarUsuario(usuario);
                item.NomeUsuario = usuario.NomeUsuario;
            }
            catch
            {
                usuario.NomeUsuario = "undefined";
            }
        }
        return listaItem;
    }

    [HttpPost]
    public async Task<JsonResult> PesquisarItensPorUsuario()
    {
        UsuarioDto usuarioDto = new UsuarioDto();
        usuarioDto.NomeUsuario = HttpContext.Session.GetString("Usuario");
        usuarioDto = await usuarioBll.PesquisarUsuario(usuarioDto);

        List<Item> listaItens = new List<Item>();
        listaItens = await marketplaceBll.PesquisarItensPorUsuario(usuarioDto.Id);
        return Json(new { success = true, lista = listaItens });
    }

    [HttpPost]
    public async Task<JsonResult> ExcluirItem(ItemDto item)
    {
        try
        {
            await marketplaceBll.ExcluirItem(item);
            return Json(new { success = true});
        }
        catch
        {
            return Json(new { success = false, message = "Falha em excluir item!" });
        }
    }
       

        
       

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
