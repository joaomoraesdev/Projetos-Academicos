using Loja_N2.DAO;
using Loja_N2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Loja_N2.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                ItemDAO dao = new ItemDAO();
                var listaItems = dao.Listagem();
                var carrinho = ObtemCarrinhoNaSession();
                //@ViewBag.TotalCarrinho = carrinho.Sum(c => c.Quantidade);
                @ViewBag.TotalCarrinho = 0;
                foreach (var c in carrinho)
                    @ViewBag.TotalCarrinho += c.Quantity;
                return View(listaItems);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Detalhes(int id)
        {
            try
            {
                List<CarrinhoViewModel> carrinho = ObtemCarrinhoNaSession();
                ItemDAO itemDao = new ItemDAO();
                var modelItem = itemDao.Consulta(id);
                CarrinhoViewModel carrinhoModel = carrinho.Find(c => c.Id == id);
                if (carrinhoModel == null)
                {
                    carrinhoModel = new CarrinhoViewModel();
                    carrinhoModel.Item_Id = modelItem.Id;
                    carrinhoModel.Item_Name = modelItem.Item_Name;
                    carrinhoModel.Item_Description = modelItem.Item_Description;
                    carrinhoModel.Price = modelItem.Price;
                    carrinhoModel.Quantity = modelItem.Quantity;
                    carrinhoModel.Category_Id = modelItem.Category_Id;
                    carrinhoModel.Category_Name = modelItem.Category_Name;
                }
                // preenche a imagem
                carrinhoModel.ImagemEmBase64 = modelItem.ImagemEmBase64;
                return View(carrinhoModel);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        private List<CarrinhoViewModel> ObtemCarrinhoNaSession()
        {
            List<CarrinhoViewModel> carrinho = new List<CarrinhoViewModel>();
            string carrinhoJson = HttpContext.Session.GetString("carrinho");
            if (carrinhoJson != null)
                carrinho = JsonConvert.DeserializeObject<List<CarrinhoViewModel>>(carrinhoJson);
            return carrinho;
        }

        public IActionResult AdicionarCarrinho(int Id, int Quantity)
        {
            try
            {
                List<CarrinhoViewModel> carrinho = ObtemCarrinhoNaSession();
                CarrinhoViewModel carrinhoModel = carrinho.Find(c => c.Id == Id);
                if (carrinhoModel != null && Quantity == 0)
                {
                    //tira do carrinho
                    carrinho.Remove(carrinhoModel);
                }
                else if (carrinhoModel == null && Quantity > 0)
                {
                    //não havia no carrinho, vamos adicionar
                    ItemDAO itemDao = new ItemDAO();
                    var modelItem = itemDao.Consulta(Id);
                    carrinhoModel = new CarrinhoViewModel();
                    carrinhoModel.Item_Id = modelItem.Id;
                    carrinhoModel.Item_Name = modelItem.Item_Name;
                    carrinhoModel.Item_Description = modelItem.Item_Description;
                    carrinhoModel.Price = modelItem.Price;
                    carrinhoModel.Quantity = modelItem.Quantity;
                    carrinhoModel.Category_Id = modelItem.Category_Id;
                    carrinhoModel.Category_Name = modelItem.Category_Name;
                    carrinho.Add(carrinhoModel);
                }
                if (carrinhoModel != null)
                    carrinhoModel.Quantity = Quantity;
                string carrinhoJson = JsonConvert.SerializeObject(carrinho);
                HttpContext.Session.SetString("carrinho", carrinhoJson);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Visualizar()
        {
            try
            {
                ItemDAO dao = new ItemDAO();
                var carrinho = ObtemCarrinhoNaSession();
                foreach (var item in carrinho)
                {
                    var itemAux = dao.Consulta(item.Item_Id);
                    item.ImagemEmBase64 = itemAux.ImagemEmBase64;
                }
                return View(carrinho);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!HelperControllers.VerificaUserLogado(HttpContext.Session))
                context.Result = RedirectToAction("Index", "Login");
            else
            {
                ViewBag.Logado = true;
                base.OnActionExecuting(context);
            }
        }

        public IActionResult EfetuarPedido()
        {
            try
            {
                using (var transacao = new System.Transactions.TransactionScope())
                {
                    OrderViewModel pedido = new OrderViewModel();
                    OrderDAO pedidoDAO = new OrderDAO();
                    pedido.Data = DateTime.Now;
                    int idPedido = pedidoDAO.Insert(pedido);
                    OrderItemsDAO itemDAO = new OrderItemsDAO();
                    var carrinho = ObtemCarrinhoNaSession();
                    foreach (var elemento in carrinho)
                    {
                        OrderItemsViewModel item = new OrderItemsViewModel();
                        item.orderId = idPedido;
                        item.orderedQuantity = elemento.Quantity;
                        item.itemId = elemento.Item_Id;
                        itemDAO.Insert(item);
                    }
                    transacao.Complete();
                }

                HttpContext.Session.Remove("carrinho");

                return RedirectToAction("Index", "Carrinho");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }
    }
}
