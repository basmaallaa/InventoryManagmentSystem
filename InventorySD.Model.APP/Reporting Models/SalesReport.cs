using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Model.APP.Reporting_Models
{
    public class SalesReport
    {
        public decimal TotalSales { get; set; }
        public List<TopSellingProduct> TopSellingProducts { get; set; }
    }
}
