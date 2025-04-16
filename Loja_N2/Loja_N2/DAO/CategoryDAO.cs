using Loja_N2.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Loja_N2.DAO
{
    public class CategoryDAO : PadraoDAO<CategoryViewModel>
    {
        protected override SqlParameter[] CriaParametros(CategoryViewModel model)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("Category_Name", model.Category_Name)
            };

            return param;
        }

        protected override CategoryViewModel MontaModel(DataRow registro)
        {
            CategoryViewModel categoria = new CategoryViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                Category_Name = registro["Category_Name"].ToString()
            };

            return categoria;
        }

        protected override void SetTabela()
        {
            Schema = "Store.";
            Tabela = "Items_Categories";
        }
    }
}
