using EC5N2B1___Curriculos.DTO;
using EC5N2B1___Curriculos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EC5N2B1___Curriculos.DAO
{
    public class CurriculoDAO
    {
        #region Inserir

        public void Inserir(CurriculoViewModel curriculo)
        {
            string sql =
            "insert into dados_Pessoais(Id_Pessoa, Nome, Cpf, Endereço, Telefone, Email, Pret_Salarial, Cargo_Pret)" +
            "values ( @Id_Pessoa, @Nome, @Cpf, @Endereco, @Telefone, @Email, @Pret_Salarial, @Cargo_Pret)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.dadosPessoais));

            sql =
            "insert into formacao_Academica(Id_Formacao, Id_Pessoa, Instituicao, Data_Inicio, Data_Termino, Descricao)" +
            "values ( @Id_Formacao, @Id_Pessoa, @Instituicao, @Data_Inicio, @Data_Termino, @Descricao)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.formacaoAcademica));

            sql =
            "insert into historico_Profissional(Id_Historico, Id_Pessoa, Empresa, Cargo, Descricao, Data_Inicio, Data_Termino)" +
            "values ( @Id_Historico, @Id_Pessoa, @Empresa, @Cargo, @Descricao, @Data_Inicio, @Data_Termino)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.historicoProfissional));

            sql =
            "insert into competencias(Id_Competencias, Id_Pessoa, Categoria, Descricao, Nivel)" +
            "values ( @Id_Competencias, @Id_Pessoa, @Categoria, @Descricao, @Nivel)";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.competencias));
        }

        #endregion Inserir  

        public void Alterar(CurriculoViewModel curriculo)
        {
            string sql =
            "update dados_Pessoais" +
            "set Nome = @Nome, " +
            "Cpf = @Cpf, " +
            "Endereco = @Endereco, " +
            "Telefone = @Telefone, " +
            "Email = @Email, " +
            "Pret_Salarial = @Pret_Salarial, " +
            "Cargo_Pret = @Cargo_Pret" +
            "where Id_Pessoa = @Id_Pessoa";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.dadosPessoais));

            sql =
            "update formacao_Academica" +
            "set Instituicao = @Instituicao, " +
            "Data_Inicio = @Data_Inicio, " +
            "Data_Termino = @Data_Termino, " +
            "Descricao = @Descricao " +
            "where Id_Formacao = @Id_Formacao";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.formacaoAcademica));

            sql =
            "update historico_Profissional" +
            "set Empresa = @Empresa, " +
            "Cargo = @Cargo, " +
            "Descricao = @Descricao, " +
            "Data_Inicio = @Data_Inicio, " +
            "Data_Termino = @Data_Termino " +
            "where Id_Historico = @Id_Historico";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.historicoProfissional));

            sql =
            "update competencias" +
            "set Empresa = @Empresa, " +
            "Categoria = @Categoria, " +
            "Descricao = @Descricao, " +
            "Nivel = @Nivel " +
            "where Id_Competencias = @Id_Competencias";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo.competencias));
        }

        public void Excluir(int id)
        {
            string sql = "delete dados_Pessoais, formacao_Academica, historico_Profissional, competencias " +
                "from dados_Pessoais " +
                "inner join formacao_Academica on dados_Pessoais.Id_Pessoa = formacao_Academica.Id_Pessoa " +
                "inner join historico_Profissional on dados_Pessoais.Id_Pessoa = historico_Profissional.Id_Pessoa " +
                "inner join competencias on dados_Pessoais.Id_Pessoa = competencias.Id_Pessoa " +
                "where Id_Pessoa = " + id;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public CurriculoViewModel Consulta(int id)
        {
            string sql = "select * from dados_Pessoais " +
                "inner join formacao_Academica on dados_Pessoais.Id_Pessoa = formacao_Academica.Id_Pessoa " +
                "inner join historico_Profissional on dados_Pessoais.Id_Pessoa = historico_Profissional.Id_Pessoa " +
                "inner join competencias on dados_Pessoais.Id_Pessoa = competencias.Id_Pessoa " +
                "where Id_Pessoa = " + id;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaCurriculo(tabela.Rows[0]);
        }
        public List<CurriculoViewModel> Listagem()
        {
            List<CurriculoViewModel> lista = new List<CurriculoViewModel>();
            string sql = "select * from dados_Pessoais " +
                "inner join formacao_Academica on dados_Pessoais.Id_Pessoa = formacao_Academica.Id_Pessoa " +
                "inner join historico_Profissional on dados_Pessoais.Id_Pessoa = historico_Profissional.Id_Pessoa " +
                "inner join competencias on dados_Pessoais.Id_Pessoa = competencias.Id_Pessoa " +
                "order by dados_Pessoais.Nome";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaCurriculo(registro));
            return lista;
        }

        //if (jogo.ValorLocacao == null) parametros[2] = new SqlParameter("valor_locacao", DBNull.Value);
        //else parametros[2] = new SqlParameter("valor_locacao", dados.ValorLocacao);

        private SqlParameter[] CriaParametros(DadosPessoais dados)
        {
            SqlParameter[] parametros = new SqlParameter[8];
            parametros[0] = new SqlParameter("Id_Pessoa", dados.Id_Pessoa);
            parametros[1] = new SqlParameter("Nome", dados.Nome);
            parametros[2] = new SqlParameter("Cpf", dados.Cpf);
            parametros[3] = new SqlParameter("Endereco", dados.Endereco);
            parametros[4] = new SqlParameter("Telefone", dados.Telefone);
            parametros[5] = new SqlParameter("Email", dados.Email);
            parametros[6] = new SqlParameter("Pret_Salarial", dados.Pret_Salarial);
            parametros[7] = new SqlParameter("Cargo_Pret", dados.Cargo_Pret);
            return parametros;
        }

        private SqlParameter[] CriaParametros(FormacaoAcademica form)
        {
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("Id_Formacao", form.Id_Formacao);
            parametros[1] = new SqlParameter("Id_Pessoa", form.Id_Pessoa);
            parametros[2] = new SqlParameter("Instituicao", form.Instituicao);
            parametros[3] = new SqlParameter("Data_Inicio", form.Data_Inicio);
            parametros[4] = new SqlParameter("Data_Termino", form.Data_Termino);
            parametros[5] = new SqlParameter("Descricao", form.Descricao);
            return parametros;
        }

        private SqlParameter[] CriaParametros(HistoricoProfissional hist)
        {
            SqlParameter[] parametros = new SqlParameter[7];
            parametros[0] = new SqlParameter("Id_Historico", hist.Id_Historico);
            parametros[1] = new SqlParameter("Id_Pessoa",   hist.Id_Pessoa);
            parametros[2] = new SqlParameter("Empresa", hist.Empresa);
            parametros[3] = new SqlParameter("Cargo", hist.Cargo);
            parametros[4] = new SqlParameter("Descricao", hist.Descricao);
            parametros[5] = new SqlParameter("Data_Inicio", hist.Data_Inicio);
            parametros[6] = new SqlParameter("Data_Termino",hist.Data_Termino);

            return parametros;
        }
        private SqlParameter[] CriaParametros(Competencias comp)
        {
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("Id_Competencias", comp.Id_Competencias);
            parametros[1] = new SqlParameter("Id_Pessoa", comp.Id_Pessoa);
            parametros[2] = new SqlParameter("Categoria", comp.Categoria);
            parametros[3] = new SqlParameter("Descricao", comp.Descricao);
            parametros[4] = new SqlParameter("Nivel", comp.Nivel);

            return parametros;
        }

        private CurriculoViewModel MontaCurriculo(DataRow registro)
        {
            CurriculoViewModel curriculo = new CurriculoViewModel();
            DadosPessoais dados = new DadosPessoais();
            FormacaoAcademica form = new FormacaoAcademica();
            HistoricoProfissional hist = new HistoricoProfissional();
            Competencias comp = new Competencias();

            dados.Id_Pessoa = Convert.ToInt32(registro["Id_Pessoa"]);
            dados.Nome = registro["Nome"].ToString();
            dados.Cpf = registro["Cpf"].ToString();
            dados.Endereco = registro["Endereco"].ToString();
            dados.Telefone = registro["Telefone"].ToString();
            dados.Email = registro["Email"].ToString();
            dados.Pret_Salarial = Convert.ToDouble(registro["Pret_Salarial"]);
            dados.Cargo_Pret = registro["Cargo_Pret"].ToString();

            form.Id_Formacao = Convert.ToInt32(registro["Id_Formacao"]);
            form.Id_Pessoa = Convert.ToInt32(registro["Id_Pessoa"]);
            form.Instituicao = registro["Instituicao"].ToString();
            form.Data_Inicio = Convert.ToDateTime(registro["formacao_Academica.Data_Inicio"]);
            form.Data_Termino = Convert.ToDateTime(registro["formacao_Academica.Data_Fim"]);
            form.Descricao = registro["formacao_Academica.Descricao"].ToString();

            hist.Id_Historico = Convert.ToInt32(registro["Id_Historico"]);
            hist.Id_Pessoa = Convert.ToInt32(registro["Id_Pessoa"]);
            hist.Empresa = registro["Empresa"].ToString();
            hist.Cargo = registro["Cargo"].ToString();
            hist.Descricao = registro["historico_Profissional.Descricao"].ToString();
            hist.Data_Inicio = Convert.ToDateTime(registro["historico_Profissional.Data_Inicio"]);
            hist.Data_Termino = Convert.ToDateTime(registro["historico_Profissional.Data_Fim"]);

            comp.Id_Competencias = Convert.ToInt32(registro["Id_Competencias"]);
            comp.Id_Pessoa = Convert.ToInt32(registro["Id_Pessoa"]);
            comp.Categoria = registro["Categoria"].ToString();
            comp.Descricao = registro["competencias.Descricao"].ToString();
            comp.Nivel = registro["Nivel"].ToString();

            curriculo.dadosPessoais = dados;
            curriculo.formacaoAcademica = form;
            curriculo.historicoProfissional = hist;
            curriculo.competencias = comp;

            return curriculo;
        }
    }
}
