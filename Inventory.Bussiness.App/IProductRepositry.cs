using Inventory.Model.APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public interface IProductRepository : IRepositry<Product>
    {
        ICollection<Product> GetProductsWithSuppliers();
    }
}