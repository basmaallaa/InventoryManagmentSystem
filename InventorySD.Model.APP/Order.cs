using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model.APP
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerID { get; set; }
        public Double TotalAmount { get; set; }

        public string? Status { get; set; }
        // Navigation property for the customer who placed the order
        public Customer Customer { get; set; }

        // Navigation property for the products in the order
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
