using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model.APP
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public Double Price { get; set; }

        // Foreign key for Supplier
        public int SupplierID { get; set; }

        // Navigation property for Supplier
        public Supplier Supplier { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
