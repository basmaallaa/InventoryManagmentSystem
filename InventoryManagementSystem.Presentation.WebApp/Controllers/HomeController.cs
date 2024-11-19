using Inventory.Business.App;
using Inventory.Presentation.App.Models;
using Inventory.Presentation.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory.Presentation.App.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ReportRepository _reportRepository;

        public HomeController(ReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var lowStockThreshold = 10; // Example threshold for low stock

        //    var lowStockProducts = await _reportRepository.GetLowStockReportAsync(lowStockThreshold);
        //    var salesReport = await _reportRepository.GetSalesReportAsync(DateTime.MinValue, DateTime.MaxValue); // Adjust date range if needed
        //    var supplierPerformanceReport = await _reportRepository.GetSupplierPerformanceReportAsync();
        //    var inventoryStatus = await _reportRepository.GetInventoryStatusAsync(lowStockThreshold);

        //    var model = new DashBoardViewModel
        //    {
        //        TotalProducts = lowStockProducts.Count, // Use appropriate method to get total products
        //        TotalSales = salesReport.TotalSales,
        //        TotalSuppliers = supplierPerformanceReport.Count, // Assuming this counts suppliers
        //        TotalInStockProducts = inventoryStatus.InStockProducts.Count,
        //        LowStockProducts = lowStockProducts
        //    };
        //    return View(model);
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
