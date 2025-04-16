using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pbl_EC8.Models;
using PBL_EC8.Bll;
using PBL_EC8;
using AspNetCoreGeneratedDocument;

namespace PBL_EC8.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UsuarioBll usuarioBll;
    private readonly JwtService jwtService;

    // Construtor injetando tanto o ILogger quanto o UsuarioBll
    public HomeController(ILogger<HomeController> logger, UsuarioBll _usuarioBll, JwtService _jwtService)
    {
        _logger = logger;
        usuarioBll = _usuarioBll;
        jwtService = _jwtService;
    }

    public IActionResult Menu()
    {
        return View();
    }

    public IActionResult Perfil()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult CadastroUsuario()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> ValidacaoLogin(string login, string senha)
    {
        string token = "";
        UsuarioDto usuario = new UsuarioDto();
        RetornoAcaoDto resultado = await usuarioBll.ValidacaoLogin(login, senha);
        if (resultado.Sucesso)
        {
            usuario.NomeUsuario = login;
            usuario = await usuarioBll.PesquisarUsuario(usuario);
            HttpContext.Session.SetString("Usuario", login);
            HttpContext.Session.SetString("ImagemPerfil", usuario.ImagemPerfil);
            token = jwtService.GenerateToken(login);
        }
        return Json(new { success = resultado.Sucesso, message = resultado.Mensagem, redirectUrl = Url.Action("ComunidadeIndex", "Comunidade"), token = token });
    }

    [HttpPost]
    public async Task<JsonResult> CadastrarUsuario(UsuarioDto dto)
    {
        RetornoAcaoDto resultado = await usuarioBll.CriarUsuario(dto);
        return Json(new { success = resultado.Sucesso, message = resultado.Mensagem, redirectUrl = Url.Action("Menu", "Home") });
    }

    [HttpGet]
    public JsonResult BuscaSessionInfos()
    {
        string nomeUsuario = HttpContext.Session.GetString("Usuario");
        string imgPerfil = HttpContext.Session.GetString("ImagemPerfil");
        return Json(new { usuario = nomeUsuario, imagemPerfil = imgPerfil });
    }

    [HttpPost]
    public async Task<JsonResult> ValidacaoPerfil()
    {
        UsuarioDto usuarioDto = new UsuarioDto();
        usuarioDto.NomeUsuario = HttpContext.Session.GetString("Usuario");
        usuarioDto = await usuarioBll.PesquisarUsuario(usuarioDto);

        if(usuarioDto.Perfil =="Premium"){
            return Json(new { success = true});
        }
        else{
            return Json(new { success = false});
        }
        
       
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
