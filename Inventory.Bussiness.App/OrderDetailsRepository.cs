using Inventory.Data.App;
using Inventory.Model.APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.App
{
    class OrderDetailsRepository : Repository<OrderProduct>, IRepositry<OrderProduct>
    {
        public InventoryContext context { get; set; }
        public OrderDetailsRepository(InventoryContext context) : base(context)
        {
            context = new InventoryContext();
        }

    }
}
