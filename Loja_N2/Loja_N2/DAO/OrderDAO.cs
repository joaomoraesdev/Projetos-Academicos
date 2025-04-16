using Loja_N2.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Loja_N2.DAO
{
    public class OrderDAO : PadraoDAO<OrderViewModel>
    {
        protected override SqlParameter[] CriaParametros(OrderViewModel model)
        {
            SqlParameter[] parametros =
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("DataCompra", model.Data)
            };
            return parametros;
        }

        protected override OrderViewModel MontaModel(DataRow registro)
        {
            OrderViewModel c = new OrderViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                Data = Convert.ToDateTime(registro["DataCompra"])
            };
            return c;
        }

        protected override void SetTabela()
        {
            Schema = "Sales.";
            Tabela = "Orders";
            ChaveIdentity = true;
        }
    }
}
