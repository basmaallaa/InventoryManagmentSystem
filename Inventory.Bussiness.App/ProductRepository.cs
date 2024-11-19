using Inventory.Data.App;
using Inventory.Model.APP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public class ProductRepository : Repository<Product>, IRepositry<Product> , IProductRepository
    {  
        public ProductRepository productrepo {  get; set; }
        public InventoryContext _context { get; set; }

        public ProductRepository(InventoryContext context) : base(context)
        {
            _context = context;
        }


        public void RestockProduct(int productId, int quantity)
        {
                Product item = productrepo.GetById(productId);
                item.Quantity += quantity;
        }

       public ICollection<Product> GetLowStockProducts(int threshold)
        {
           
            List<Product> products = productrepo.GetALL().Where(p=> p.Quantity < threshold).ToList();
            return products;

        }


        // Specialized method to include Supplier when fetching Products
        public ICollection<Product> GetProductsWithSuppliers()
        {
            return _context.Products.Include(p => p.Supplier).ToList(); // Eagerly load Supplier
        }


    }
}
