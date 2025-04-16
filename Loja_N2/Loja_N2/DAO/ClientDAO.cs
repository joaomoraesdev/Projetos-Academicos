using Loja_N2.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Loja_N2.DAO
{
    public class ClientDAO : PadraoDAO<ClientViewModel>
    {
        protected override SqlParameter[] CriaParametros(ClientViewModel model)
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("Id", model.Id),
                new SqlParameter("First_Name", model.First_Name),
                new SqlParameter("Last_Name", model.Last_Name),
                new SqlParameter("Email", model.Email),
                new SqlParameter("Telephone", model.Telephone),
                new SqlParameter("Localidade", model.Localidade),
                new SqlParameter("Uf", model.Uf),
                new SqlParameter("Cep", model.Cep),
                new SqlParameter("Logradouro", model.Logradouro),
                new SqlParameter("Birth_Date", model.Birth_Date),
                new SqlParameter("Client_Password", model.Client_Password),
            };

            return param;
        }

        protected override ClientViewModel MontaModel(DataRow registro)
        {
            ClientViewModel cliente = new ClientViewModel()
            {
                Id = Convert.ToInt32(registro["Id"]),
                First_Name = Convert.ToString(registro["First_Name"]),
                Last_Name = Convert.ToString(registro["Last_Name"]),
                Email = Convert.ToString(registro["Email"]),
                Telephone = Convert.ToString(registro["Telephone"]),
                Localidade = Convert.ToString(registro["Localidade"]),
                Uf = Convert.ToString(registro["Uf"]),
                Cep = Convert.ToString(registro["Cep"]),
                Logradouro = Convert.ToString(registro["Logradouro"]),
                Birth_Date = Convert.ToDateTime(registro["Birth_Date"]),
                Client_Password = Convert.ToString(registro["Client_Password"])
            };

            return cliente;
        }

        protected override void SetTabela()
        {
            Schema = "Store.";
            Tabela = "Clients";
        }
        public virtual ClientViewModel ConsultaLogin(string email)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("Email", HelperDAO.NullAsDbNull(email)),
            };

            var tabela = HelperDAO.ExecutaProcSelect("spVerifica_Login", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

    }
}
