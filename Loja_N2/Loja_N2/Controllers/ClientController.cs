using Loja_N2.DAO;
using Loja_N2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace Loja_N2.Controllers
{
    public class ClientController : PadraoController<ClientViewModel>
    {
        public ClientController()
        {
            DAO = new ClientDAO();
            NomeViewIndex = "ConsultaCliente";
            NomeViewForm = "CadastroCliente";
        }

        protected override void PreencheDadosParaView(string Operacao, ClientViewModel model)
        {
            model.Birth_Date = DateTime.Now;
        }


        protected override void ValidaDados(ClientViewModel model, string operacao)
        {
            //base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.First_Name))
                ModelState.AddModelError("First_Name", "Preencha o primeiro nome.");
            if (string.IsNullOrEmpty(model.Last_Name))
                ModelState.AddModelError("Last_Name", "Preencha o último nome");
            if (string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Preencha o Email");
            if (string.IsNullOrEmpty(model.Telephone))
                ModelState.AddModelError("Telephone", "Preencha o telefone");
            if (string.IsNullOrEmpty(model.Localidade))
                ModelState.AddModelError("Localidade", "Informe a cidade");
            if (string.IsNullOrEmpty(model.Uf))
                ModelState.AddModelError("Uf", "Informe o estado");
            if (string.IsNullOrEmpty(model.Cep))
                ModelState.AddModelError("Cep", "Digite o CEP");
            if (string.IsNullOrEmpty(model.Logradouro))
                ModelState.AddModelError("Logradouro", "Digite a rua");
            if (model.Birth_Date > DateTime.Now)
                ModelState.AddModelError("Birth_Date", "Data Inválida");
            if (string.IsNullOrEmpty(model.Client_Password))
                ModelState.AddModelError("Client_Password", "Digite uma senha");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool create = context.ActionDescriptor.DisplayName.Contains("Create");
            bool save = context.ActionDescriptor.DisplayName.Contains("Save");

            if (create)
                RedirectToAction("Create", "Client");
            else if (save)
                RedirectToAction("Save", "Client");
            else
                base.OnActionExecuting(context);
        }
    }
}
