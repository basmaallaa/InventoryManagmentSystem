using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model.APP.Reporting_Models
{
    public class SupplierPerformanceReport
    {
        public string SupplierName { get; set; }
        public double AverageDeliveryTime { get; set; }
        public int TotalProductsSupplied { get; set; }
    }
}
