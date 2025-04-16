using Microsoft.AspNetCore.Http;
using System;

namespace Loja_N2.Models
{
    public class ClientViewModel : PadraoViewModel
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public DateTime Birth_Date { get; set; }
        public string Client_Password { get; set; }
    }
}
