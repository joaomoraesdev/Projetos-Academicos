using System;

namespace EC5N2B1___Curriculos.DTO
{
    public class HistoricoProfissional
    {
        public int Id_Historico { get; set; }
        public int Id_Pessoa { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data_Inicio { get; set; }
        public DateTime Data_Termino { get; set; }
    }
}
