using System.Data.SqlClient;

namespace EC5N2B1___Curriculos

{
    public static class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            string strCon = @"Data Source=DESKTOP-J1B4VMI\SQLEXPRESS;Initial Catalog=N2B1;integrated security=true";
            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
