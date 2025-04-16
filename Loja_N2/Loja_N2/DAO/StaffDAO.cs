using Loja_N2.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Loja_N2.DAO
{
    public class StaffDAO : PadraoDAO<StaffViewModel>
    {
        protected override SqlParameter[] CriaParametros(StaffViewModel model)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("First_Name", model.First_Name),
                new SqlParameter("Last_Name", model.Last_Name),
                new SqlParameter("Cpf", model.Cpf),
                new SqlParameter("Staff_Password", model.Staff_Password)
            };

            return param;
        }

        protected override StaffViewModel MontaModel(DataRow registro)
        {
            StaffViewModel func = new StaffViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                First_Name = Convert.ToString(registro["First_Name"]),
                Last_Name = Convert.ToString(registro["Last_Name"]),
                Cpf = Convert.ToString(registro["Cpf"]),
                Staff_Password = Convert.ToString(registro["Staff_Password"])
            };

            return func;
        }

        protected override void SetTabela()
        {
            Schema = "Store.";
            Tabela = "Staffs";
        }

        public virtual StaffViewModel ConsultaLogin(string cpf)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("Cpf", HelperDAO.NullAsDbNull(cpf)),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spVerifica_LoginFunc", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }
    }
}
