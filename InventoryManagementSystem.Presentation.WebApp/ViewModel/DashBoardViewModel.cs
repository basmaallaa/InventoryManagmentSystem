using Inventory.Model.APP;

namespace Inventory.Presentation.App.ViewModel
{
    public class DashBoardViewModel
    {
        public int TotalProducts { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalInStockProducts { get; set; }
        public List<Product> LowStockProducts { get; set; } // Assuming Product is your model class
    }
}
