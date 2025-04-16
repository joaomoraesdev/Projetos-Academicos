using EC5N2B1___Curriculos.DAO;
using EC5N2B1___Curriculos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EC5N2B1___Curriculos.Controllers
{
    public class CurriculoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cadastro()
        {

            try
            {
                //ViewBag.Operacao = "I";
                CurriculoViewModel curriculo = new CurriculoViewModel();
                curriculo.dadosPessoais = new DTO.DadosPessoais();
                curriculo.formacaoAcademica = new DTO.FormacaoAcademica();
                curriculo.historicoProfissional = new DTO.HistoricoProfissional();
                curriculo.competencias = new DTO.Competencias();
                return View("Form", curriculo);
            }
            catch (Exception erro)
            {
                return View("error",
                    new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Visualizar()
        {
            CurriculoDAO dao = new CurriculoDAO();
            var lista = dao.Listagem();
            return View("Visualizacao", lista);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Salvar(CurriculoViewModel curriculo)
        {
            curriculo.historicoProfissional.Id_Pessoa = curriculo.dadosPessoais.Id_Pessoa;
            curriculo.formacaoAcademica.Id_Pessoa = curriculo.dadosPessoais.Id_Pessoa;
            curriculo.competencias.Id_Pessoa = curriculo.dadosPessoais.Id_Pessoa;

            try
            {
                CurriculoDAO dao = new CurriculoDAO();
                dao.Inserir(curriculo);
                return RedirectToAction("index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
