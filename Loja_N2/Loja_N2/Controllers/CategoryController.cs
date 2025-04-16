using Loja_N2.DAO;
using Loja_N2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Loja_N2.Controllers
{
    public class CategoryController : PadraoController<CategoryViewModel>
    {
        public CategoryController()
        {
            DAO = new CategoryDAO();
            NomeViewIndex = "ConsultaCategoria";
            NomeViewForm = "CadastroCategoria";
        }

        protected override void ValidaDados(CategoryViewModel model, string operacao)
        {
            //base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Category_Name))
                ModelState.AddModelError("Category_Name", "Preencha a Categoria.");
        }
    }
}
