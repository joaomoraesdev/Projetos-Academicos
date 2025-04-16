namespace Loja_N2.Models
{
    public class OrderItemsViewModel : PadraoViewModel
    {
        public int itemId { get; set; }
        public int orderId { get; set; }
        public int orderedQuantity { get; set; }
    }
}
