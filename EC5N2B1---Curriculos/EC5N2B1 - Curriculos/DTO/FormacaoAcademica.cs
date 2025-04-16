using System;

namespace EC5N2B1___Curriculos.DTO
{
    public class FormacaoAcademica
    {
        public int Id_Formacao { get; set; }
        public int Id_Pessoa { get; set; }
        public string Instituicao { get; set; }
        public DateTime Data_Inicio { get; set; }
        public DateTime Data_Termino { get; set; }
        public string Descricao { get; set; }
    }
}
