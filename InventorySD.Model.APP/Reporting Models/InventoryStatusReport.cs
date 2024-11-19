using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model.APP.Reporting_Models
{
    public class InventoryStatusReport
    {
        public List<Product> InStockProducts { get; set; }
        public List<Product> LowStockProducts { get; set; }
    }
}
