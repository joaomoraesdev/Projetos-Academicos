using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja_N2.DAO;
using Loja_N2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Loja_N2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FazLogin(ClientViewModel usuario)
        {
            //Este é apenas um exemplo, aqui você deve consultar na sua tabela de usuários
            //se existe esse usuário e senha
            ClientDAO DAO = new ClientDAO();

            ClientViewModel clientAux = new ClientViewModel();

            clientAux = DAO.ConsultaLogin(usuario.Email);

            if (clientAux != null && clientAux.Client_Password == usuario.Client_Password)
            {
                //var clientJson = JsonConvert.SerializeObject(clientAux);
                HttpContext.Session.SetString("Client", usuario.Id.ToString());
                return RedirectToAction("Index", "Carrinho");
            }

            if (clientAux == null)
                ViewBag.Erro = "Usuário inválido!";
            else
                ViewBag.Erro = "Senha inválida!";


            return View("Index", usuario);

            /*
             */
        }
        public IActionResult FazLoginFunc(StaffViewModel func)
        {
            StaffDAO DAO = new StaffDAO();

            StaffViewModel funcAux = new StaffViewModel();

            funcAux = DAO.ConsultaLogin(func.Cpf);

            if (funcAux != null && funcAux.Staff_Password == func.Staff_Password)
            {
                var funcJson = JsonConvert.SerializeObject(funcAux);
                HttpContext.Session.SetString("Staff", funcJson);
                return RedirectToAction("Index", "Item");
            }

            if (funcAux == null)
                ViewBag.Erro = "Funcionário inválido!";
            else
                ViewBag.Erro = "Senha inválida!";


            return View("Index", func);
        }

            public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult CreateClient()
        {
            return RedirectToAction("Create", "Client");
        }
    }
}
