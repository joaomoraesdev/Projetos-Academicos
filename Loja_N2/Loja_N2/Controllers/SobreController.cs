using Loja_N2.DAO;
using Loja_N2.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Loja_N2.Controllers
{
    public class SobreController : Controller
    {
        public IActionResult Sobre()
        {
            try
            {
                return View("Sobre");
            }
            catch (Exception erro)
            {
                return View("error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
