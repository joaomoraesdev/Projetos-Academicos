using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_N2.Models
{
    public class ItemViewModel : PadraoViewModel
    {
        public string Item_Name { get; set; }
        public string Item_Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }

        /// <summary> 
        /// Imagem recebida do form pelo controller 
        /// </summary> 
        public IFormFile Imagem { get; set; }

        /// <summary> 
        /// Imagem em bytes pronta para ser salva 
        /// </summary> 
        public byte[] ImagemEmByte { get; set; }

        /// <summary> 
        /// Imagem usada para ser enviada ao form no formato para ser exibida 
        /// </summary> 
        public string ImagemEmBase64
        {
            get
            {
                if (ImagemEmByte != null)
                    return Convert.ToBase64String(ImagemEmByte);
                else
                    return string.Empty;
            }
        }
    }
}
