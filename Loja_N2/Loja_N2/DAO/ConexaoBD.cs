using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_N2.DAO
{
    public static class ConexaoBD
    {
        /// <summary>
        /// Método Estático que retorna um conexao aberta com o BD
        /// </summary>
        /// <returns>Conexão aberta</returns>
        public static SqlConnection GetConexao()
        {
            //conexão na facul
            string strCon = "Data Source=LOCALHOST; Database=LojaN2; integrated security = true";

            //conexão em casa (sqlexpression)
            //string strCon = @"Data Source=DESKTOP-J1B4VMI\SQLEXPRESS; Database=LojaN2; integrated security = true";

            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
