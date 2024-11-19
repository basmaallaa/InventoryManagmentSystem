using Inventory.Data.App;
using Inventory.Model.APP;
using Inventory.Model.APP.Reporting_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public class ReportRepository : IReportRepository
    {
        private readonly InventoryContext _context;

        //get object from context 
        public ReportRepository(InventoryContext context)
        {
            _context = context;
        }


        // 1. Get Low Stock Report
        public async Task<List<Product>> GetLowStockReportAsync(int threshold)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.Quantity < threshold)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve low stock report: ");
            }
        }


        // 2. Get Sales Report
        public async Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                //Queries to get Total Sales of orders
                var totalSales = await _context.Orders
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .SelectMany(o => o.OrderProducts)
                    .SumAsync(op => op.Quantity * op.Price);

                //Queries to List Top Selling Products List
                var topSellingProducts = await _context.OrderProducts
                    .Where(op => op.Order.OrderDate >= startDate && op.Order.OrderDate <= endDate)
                    .GroupBy(op => op.Product.Name)
                    .Select(g => new TopSellingProduct
                    {
                        ProductName = g.Key,
                        TotalQuantitySold = g.Sum(op => op.Quantity)
                    })
                    .OrderByDescending(t => t.TotalQuantitySold)
                    .Take(5)
                    .ToListAsync();

                return new SalesReport
                {
                    TotalSales = (decimal)totalSales,
                    TopSellingProducts = topSellingProducts
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve sales report: ");
            }
        }


        // 3. Get Supplier Performance Report
        public async Task<List<SupplierPerformanceReport>> GetSupplierPerformanceReportAsync()
        {
            try
            {
                return await _context.Suppliers
                    .Select(s => new SupplierPerformanceReport
                    {
                        SupplierName = s.SupplierName,
                        TotalProductsSupplied = _context.Products.Count(p => p.SupplierID == s.SupplierID)
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve supplier performance report: ");
            }
        }


        // 4. Get Inventory Status
        public async Task<InventoryStatusReport> GetInventoryStatusAsync(int threshold)
        {
            try
            {
                var inStockProducts = await _context.Products
                    .Where(p => p.Quantity > 0)
                    .ToListAsync();

                var lowStockProducts = await GetLowStockReportAsync(threshold);

                return new InventoryStatusReport
                {
                    InStockProducts = inStockProducts,
                    LowStockProducts = lowStockProducts
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve inventory status: ");
            }
        }

    }
}
