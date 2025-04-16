namespace Loja_N2.Models
{
    public class CarrinhoViewModel : PadraoViewModel
    {
        public int Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string Item_Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public string ImagemEmBase64 { get; set; }
    }
}
