namespace Inventory.Presentation.App.ViewModel
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
        public double TotalAmount { get; set; }

        // List of products in the order
        public List<OrderProductDetails> Products { get; set; }
    }
}
