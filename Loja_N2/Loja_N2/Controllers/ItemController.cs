using Loja_N2.DAO;
using Loja_N2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Data.SqlTypes;

namespace Loja_N2.Controllers
{
    public class ItemController : PadraoController<ItemViewModel>
    {
        public ItemController()
        {
            DAO = new ItemDAO();
            NomeViewIndex = "ConsultaItem";
            NomeViewForm = "CadastroItem";
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            else
                return null;
        }

        protected override void PreencheDadosParaView(string Operacao, ItemViewModel model)
        {
            base.PreencheDadosParaView(Operacao, model);
            //if (Operacao == "I")
            //    model.DataNascimento = DateTime.Now;

            //PreparaListaCidadesParaCombo();
            PreparaListaCategoriasParaCombo();
        }

        protected override void ValidaDados(ItemViewModel model,
                                            string operacao)
        {
            //base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Item_Name))
                ModelState.AddModelError("Item_Name", "Preencha o nome.");
            if (string.IsNullOrEmpty(model.Item_Description))
                ModelState.AddModelError("Item_Description", "Preencha a descrição.");
            if (model.Price < 0)
                ModelState.AddModelError("Price", "Campo obrigatório.");
            if (model.Quantity < 0)
                ModelState.AddModelError("Quantity", "Digite uma quantidade válida.");
            if (model.Category_Id <= 0)
                ModelState.AddModelError("Id", "Informe a categoria");

            //Imagem será obrigatio apenas na inclusão. 
            //Na alteração iremos considerar a que já estava salva.
            if (model.Imagem == null && operacao == "I")
                ModelState.AddModelError("Imagem", "Escolha uma imagem.");

            if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Imagem", "Imagem limitada a 2 mb.");

            if (ModelState.IsValid)
            {
                //na alteração, se não foi informada a imagem, iremos manter a que já estava salva.
                if (operacao == "A" && model.Imagem == null)
                {
                    ItemViewModel item = DAO.Consulta(model.Id);
                    model.ImagemEmByte = item.ImagemEmByte;
                }
                else
                {
                    model.ImagemEmByte = ConvertImageToByte(model.Imagem);
                }
            }
        }

        private void PreparaListaCategoriasParaCombo()
        {
            CategoryDAO catDao = new CategoryDAO();
            var categorias = catDao.Listagem();
            List<SelectListItem> listaCat = new List<SelectListItem>();

            listaCat.Add(new SelectListItem("Selecione uma categoria...", "0"));
            foreach (var cat in categorias)
            {
                SelectListItem item = new SelectListItem(cat.Category_Name, cat.Id.ToString());
                listaCat.Add(item);
            }
            ViewBag.Categorias = listaCat;
        }

        public virtual IActionResult Dashboards()
        {
            try
            {
                DashboardViewModel model = new DashboardViewModel();
                ItemDAO dao = new ItemDAO();
                List<ItemViewModel> estoque = new List<ItemViewModel>();
                List<ItensVendidosViewModel> vendidos = new List<ItensVendidosViewModel>();

                List<ItemViewModel> items = dao.Listagem().OrderByDescending(o => o.Quantity).ToList();

                if (items != null)
                {
                    ConverterItensEstoque(items, estoque, model);
                }

                List<ItensVendidosViewModel> vendas = dao.PesquisarMaisVendidos().OrderBy(o => o.QtdVendida).ToList();

                if (vendas != null)
                {
                    ConverterMenosVendidos(vendas, vendidos, model);

                    ConverterMaisVendidos(vendas, vendidos, model);
                }

                return View("Dashboards", model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private void ConverterMenosVendidos(List<ItensVendidosViewModel> vendas, List<ItensVendidosViewModel> vendidos, DashboardViewModel model)
        {
            for (int i = 0; i < 3; i++)
            {
                vendidos.Add(vendas[i]);
            }

            model.MenosVendido1 = vendidos[0].Nome;
            model.QtdMenosVendido1 = vendidos[0].QtdVendida;

            model.MenosVendido2 = vendidos[1].Nome;
            model.QtdMenosVendido2 = vendidos[1].QtdVendida;

            model.MenosVendido3 = vendidos[2].Nome;
            model.QtdMenosVendido3 = vendidos[2].QtdVendida;
        }

        private void ConverterMaisVendidos(List<ItensVendidosViewModel> vendas, List<ItensVendidosViewModel> vendidos, DashboardViewModel model)
        {
            vendas = vendas.OrderByDescending(o => o.QtdVendida).ToList();
            vendidos = new List<ItensVendidosViewModel>();

            for (int i = 0; i < 3; i++)
            {
                vendidos.Add(vendas[i]);
            }

            model.MaisVendido1 = vendidos[0].Nome;
            model.QtdMaisVendido1 = vendidos[0].QtdVendida;

            model.MaisVendido2 = vendidos[1].Nome;
            model.QtdMaisVendido2 = vendidos[1].QtdVendida;

            model.MaisVendido3 = vendidos[2].Nome;
            model.QtdMaisVendido3 = vendidos[2].QtdVendida;
        }

        private void ConverterItensEstoque(List<ItemViewModel> items, List<ItemViewModel> estoque, DashboardViewModel model)
        {
            for (int i = 0; i < 3; i++)
            {
                estoque.Add(items[i]);
            }

            model.Item1 = estoque[0].Item_Name;
            model.Qtd1 = estoque[0].Quantity;

            model.Item2 = estoque[1].Item_Name;
            model.Qtd2 = estoque[1].Quantity;

            model.Item3 = estoque[2].Item_Name;
            model.Qtd3 = estoque[2].Quantity;

        }
    }
}
