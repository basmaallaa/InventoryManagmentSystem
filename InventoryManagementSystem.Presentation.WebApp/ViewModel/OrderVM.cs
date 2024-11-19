using Inventory.Model.APP;

namespace Inventory.Presentaion.App.ViewModel
{
    public class OrderVM
    {
        public class OrderViewModel
        {
            public int OrderID { get; set; }
            public DateTime OrderDate { get; set; }
            public string CustomerName { get; set; }
            public double TotalAmount { get; set; }
            public string Status { get; set; }
        }

        public class OrderProductViewModel
        {
            public int OrderProductID { get; set; }
            public int ProductID { get; set; }
            public string? ProductName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public bool IsSelected { get; set; }
        }


        public class CreateOrderViewModel
        {
            public int CustomerID { get; set; }
            public DateTime OrderDate { get; set; }
            public string Status { get; set; }
            public List<OrderProductViewModel> SelectedProducts { get; set; } = new List<OrderProductViewModel>();
            public List<Customer> Customers { get; set; } = new List<Customer>();
            public List<Product> Products { get; set; } = new List<Product>();
        }





        public class EditOrderViewModel : CreateOrderViewModel
        {
            public int OrderID { get; set; }
        }


        /*public class OrderDetailsViewModel
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

        public class OrderProductDetails
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        }*/

    }
}
