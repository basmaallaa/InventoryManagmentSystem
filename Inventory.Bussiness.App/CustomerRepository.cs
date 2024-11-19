using Inventory.Data.App;
using Inventory.Model.APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    public class CustomerRepository : Repository<Customer>, IRepositry<Customer>
    {
        public InventoryContext context { get; set; }
        public CustomerRepository(InventoryContext context) : base(context)
        {
            context = new InventoryContext();
        }
    }
}
