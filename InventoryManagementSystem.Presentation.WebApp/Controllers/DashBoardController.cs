using Inventory.Business.App;
using Inventory.Presentation.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InventoryManagementSystem.Presentation.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ReportRepository _reportRepository;

        public DashboardController(ReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IActionResult> Index()
        {
            var lowStockThreshold = 10; // Define your threshold for low stock

           
                // Fetch reports from the repository
                var lowStockProducts = await _reportRepository.GetLowStockReportAsync(lowStockThreshold);
                var salesReport = await _reportRepository.GetSalesReportAsync(DateTime.MinValue, DateTime.MaxValue);
                var supplierPerformanceReport = await _reportRepository.GetSupplierPerformanceReportAsync();
                var inventoryStatus = await _reportRepository.GetInventoryStatusAsync(lowStockThreshold);

                // Construct the DashboardViewModel
                var model = new DashBoardViewModel
                {
                    TotalProducts = lowStockProducts.Count, // Count of low stock products
                    TotalSales = salesReport.TotalSales, // Total sales amount
                    TotalSuppliers = supplierPerformanceReport.Count, // Count of suppliers
                    TotalInStockProducts = inventoryStatus.InStockProducts.Count, // Count of in-stock products
                    LowStockProducts = lowStockProducts // List of low stock products
                };

                return View(model); // Return the view with the model
        }
            
    }
}

