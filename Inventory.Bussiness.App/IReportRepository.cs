using Inventory.Model.APP;
using Inventory.Model.APP.Reporting_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public interface IReportRepository
    {
        Task<List<Product>> GetLowStockReportAsync(int threshold);
        Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<List<SupplierPerformanceReport>> GetSupplierPerformanceReportAsync();
        Task<InventoryStatusReport> GetInventoryStatusAsync(int lowStockThreshold);
    }
}
