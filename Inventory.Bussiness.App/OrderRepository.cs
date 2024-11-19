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

    public class OrderRepository : Repository<Order>, IRepositry<Order>
    {
        public InventoryContext _context { get; set; }

        public OrderRepository(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public Order GetById(int id)
        {
            // Ensure related OrderProducts are included
            return _context.Orders
                           .Include(o => o.OrderProducts)   // Includes related products
                           .FirstOrDefault(o => o.OrderID == id);  // Fetches order by ID
        }

        public async Task<bool> CreateOrderAsync(Order order, List<OrderProduct> orderDetails)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var customer = await _context.Customers.FindAsync(order.CustomerID);
                    if (customer == null)
                    {
                        throw new Exception("Customer not found.");
                    }
                    // Step 2: Add the Order
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync(); // Save to generate OrderId
                    foreach (var detail in orderDetails)
                    {
                        var product = await _context.Products.FindAsync(detail.ProductID);
                        if (product == null)
                        {
                            throw new Exception($"Product with ID {detail.ProductID} not found.");
                        }

                        if (product.Quantity < detail.Quantity)
                        {
                            throw new Exception($"Not enough stock for product: {product.Name}");
                        }

                        // Deduct product stock
                        product.Quantity -= detail.Quantity;

                        // Link the OrderDetails to the Order
                        detail.OrderID = order.OrderID;
                        _context.OrderProducts.Add(detail);
                    }
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Order creation failed. " + ex.Message);
                }
            }
        }


        public async Task<bool> CancelOrderAsync(int orderId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    var order = await _context.Orders.FindAsync(orderId);
                    if (order == null)
                    {
                        throw new Exception("Order not found.");
                    }

                    // Mark the order as canceled
                    order.Status = "Canceled";
                    _context.Orders.Update(order);

                    // Restock the products associated with this order
                    var orderDetails = await _context.OrderProducts
                                                       .Where(od => od.OrderID == orderId)
                                                       .ToListAsync();

                    foreach (var detail in orderDetails)
                    {
                        var product = await _context.Products.FindAsync(detail.ProductID);
                        if (product != null)
                        {
                            // Restock the product
                            product.Quantity += detail.Quantity;
                        }
                    }


                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Order cancellation failed. " + ex.Message);
                }
            }
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer not found.");
            }

            var orders = await _context.Orders
                                         .Where(o => o.CustomerID == customerId)
                                         .Include(o => o.OrderProducts)
                                         .ThenInclude(od => od.Product)
                                         .ToListAsync();

            return orders;
        }

    }
}
