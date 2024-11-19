using Inventory.Data.App;
using Inventory.Model.APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public class SupplierRepository : Repository<Supplier>, IRepositry<Supplier>
    {
        public InventoryContext context { get; set; }
        public SupplierRepository(InventoryContext context) : base(context)
        {
            context = new InventoryContext();
        }

    }
}
