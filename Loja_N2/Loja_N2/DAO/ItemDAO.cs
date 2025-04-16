using Loja_N2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Loja_N2.DAO
{
    public class ItemDAO : PadraoDAO<ItemViewModel>
    {
        protected override SqlParameter[] CriaParametros(ItemViewModel item)
        {
            object imgByte = item.ImagemEmByte;
            if (imgByte == null)
                imgByte = DBNull.Value;

            SqlParameter[] p = new SqlParameter[]
            {
                new SqlParameter("Id", item.Id),
                new SqlParameter("Item_Name", item.Item_Name),
                new SqlParameter("Item_Description", item.Item_Description),
                new SqlParameter("Price", item.Price),
                new SqlParameter("Quantity", item.Quantity),
                new SqlParameter("Category_Id", item.Category_Id),
                new SqlParameter("Imagem", imgByte)
            };

            return p;
        }

        protected override ItemViewModel MontaModel(DataRow registro)
        {
            ItemViewModel item = new ItemViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                Item_Name = registro["Item_Name"].ToString(),
                Item_Description = registro["Item_Description"].ToString(),
                Price = Convert.ToDouble(registro["Price"]),
                Quantity = Convert.ToInt32(registro["Quantity"]),
                Category_Id = Convert.ToInt32(registro["Category_Id"])
            };

            if (registro["imagem"] != DBNull.Value)
                item.ImagemEmByte = registro["imagem"] as byte[];
            if (registro.Table.Columns.Contains("CategoryName"))
                item.Category_Name = registro["CategoryName"].ToString();

            return item;
        }

        protected override void SetTabela()
        {
            Schema = "Store.";
            Tabela = "Items";
            NomeSpListagem = "spListagem_Items";
        }

        public List<ItensVendidosViewModel> PesquisarMaisVendidos()
        {
            List<ItensVendidosViewModel> retorno = new List<ItensVendidosViewModel>();

            var tabela = HelperDAO.ExecutaProcSelect("spItens_Vendidos", null);

            if (tabela.Rows.Count == 0)
                return null;
            else
            {
                foreach (DataRow registro in tabela.Rows)
                    retorno.Add(MontaModelMaisVendidos(registro));
            }

            return retorno;
        }

        private ItensVendidosViewModel MontaModelMaisVendidos(DataRow registro)
        {
            ItensVendidosViewModel item = new ItensVendidosViewModel();
            item.Nome = registro["Item_Name"].ToString();
            item.QtdVendida = Convert.ToInt32(registro["Ordered_Quantity"]);

            return item;
        }
    }
}
