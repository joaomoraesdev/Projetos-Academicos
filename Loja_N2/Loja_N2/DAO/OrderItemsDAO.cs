using Loja_N2.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Loja_N2.DAO
{
    public class OrderItemsDAO : PadraoDAO<OrderItemsViewModel>
    {
        protected override SqlParameter[] CriaParametros(OrderItemsViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("Item_Id", model.itemId),
                new SqlParameter("Order_Id", model.orderId),
                new SqlParameter("Ordered_Quantity", model.orderedQuantity)
            };
            return parametros;
        }

        protected override OrderItemsViewModel MontaModel(DataRow registro)
        {
            OrderItemsViewModel c = new OrderItemsViewModel()
            {
                Id = Convert.ToInt32(registro["id"]),
                itemId = Convert.ToInt32(registro["Item_Id"]),
                orderId = Convert.ToInt32(registro["Order_Id"]),
                orderedQuantity = Convert.ToInt32(registro["Ordered_Quantity"]),
            };
            return c;
        }

        protected override void SetTabela()
        {
            Schema = "Sales.";
            Tabela = "Orders_Items";
            ChaveIdentity = true;
        }
    }
}
