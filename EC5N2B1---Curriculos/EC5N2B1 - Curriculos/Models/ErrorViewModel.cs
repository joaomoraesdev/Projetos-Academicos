using System;

namespace EC5N2B1___Curriculos.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string erro)
        {
            this.Erro = erro;
        }
        public ErrorViewModel()
        {

        }
        public string Erro { get; set; }
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
