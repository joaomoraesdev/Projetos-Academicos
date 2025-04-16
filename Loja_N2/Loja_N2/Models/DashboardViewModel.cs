namespace Loja_N2.Models
{
    public class DashboardViewModel : PadraoViewModel
    {
        //Dashboard Estoque
        public string Item1 { get; set; }
        public int Qtd1 { get; set; }
        public string Item2 { get; set; }
        public int Qtd2 { get; set; }
        public string Item3 { get; set; }
        public int Qtd3 { get; set; }

        //Dashboard Mais Vendidos
        public string MaisVendido1 { get; set; }
        public int QtdMaisVendido1 { get; set; }
        public string MaisVendido2 { get; set; }
        public int QtdMaisVendido2 { get; set; }
        public string MaisVendido3 { get; set; }
        public int QtdMaisVendido3 { get; set; }

        //Dashboard Menos Vendidos
        public string MenosVendido1 { get; set; }
        public int QtdMenosVendido1 { get; set; }
        public string MenosVendido2 { get; set; }
        public int QtdMenosVendido2 { get; set; }
        public string MenosVendido3 { get; set; }
        public int QtdMenosVendido3 { get; set; }
    }
}
