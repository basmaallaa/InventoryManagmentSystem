using Inventory.Business.App;
using Inventory.Model.APP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Inventory.Presentaion.App.ViewModel;
using Inventory.Presentation.App.ViewModel;
using static Inventory.Presentaion.App.ViewModel.OrderVM;
using NuGet.Packaging;


namespace Inventory.Presentaion.App.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepositry<Order> _orderRepo;
        private readonly IRepositry<Product> _productRepo;
        private readonly IRepositry<Customer> _customerRepo;

        public OrderController(IRepositry<Order> orderRepo, IRepositry<Product> productRepo, IRepositry<Customer> customerRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
        }

        // GET: OrderController
        public ActionResult Index()
        {
            var orders = _orderRepo.GetALL();
            var model = orders.Select(order => new OrderViewModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                // Add null check for Customer and its Name property
                CustomerName = order.Customer != null ? order.Customer.Name : " Customer",
                TotalAmount = order.TotalAmount,
                Status = order.Status
            }).ToList();

            return View(model);
        }


        // Get details of a specific order
        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderDetailsViewModel
            {
                OrderID = order.OrderID,
                OrderDate = order.OrderDate,
                CustomerName = order.Customer?.Name ?? " Customer", // Using null-conditional operator
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Products = order.OrderProducts?.Select(op => new OrderProductDetails
                {
                    ProductID = op.ProductID,
                    ProductName = op.Product?.Name ?? " Product", // Using null-conditional operator
                    Quantity = op.Quantity,
                    Price = op.Price
                }).ToList() ?? new List<OrderProductDetails>() // Default to an empty list if OrderProducts is null
            };


            return View(model);
        }

        // Render form for creating a new order
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateOrderViewModel
            {
                Customers = _customerRepo.GetALL().ToList(),
                Products = _productRepo.GetALL().ToList(),
                SelectedProducts = _productRepo.GetALL().Select(p => new OrderProductViewModel
                {
                    ProductID = p.ProductID,
                    ProductName = p.Name,
                    IsSelected = false, // Initialize to false
                    Quantity = 0, // Default quantity
                    Price = p.Price // Default price
                }).ToList()
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateOrderViewModel model)
        {
            // Populate the Customers and Products for the view in case of validation failure
            model.Customers = _customerRepo.GetALL().ToList();
            model.Products = _productRepo.GetALL().ToList();

            if (ModelState.IsValid)
            {
                // Filter the selected products based on IsSelected property
                var selectedProducts = model.SelectedProducts
                    .Where(p => p.IsSelected && p.ProductID > 0) // Ensure ProductID is valid
                    .ToList();

                // Debugging line
                Console.WriteLine($"Selected Products Count: {selectedProducts.Count}");

                // Validate that at least one product has been selected
                if (!selectedProducts.Any())
                {
                    ModelState.AddModelError(string.Empty, "At least one product must be selected.");
                    return View(model); // Return the view with validation errors
                }

                // Validate Product IDs
                var existingProductIds = _productRepo.GetALL().Select(p => p.ProductID).ToList();
                var invalidProductIds = selectedProducts.Select(sp => sp.ProductID).Where(id => !existingProductIds.Contains(id)).ToList();
                if (invalidProductIds.Any())
                {
                    // Log the invalid ProductIDs or handle accordingly
                    ModelState.AddModelError(string.Empty, $"Invalid ProductIDs: {string.Join(", ", invalidProductIds)}");
                    return View(model); // Return the view with validation errors
                }

                // Create the new order
                var newOrder = new Order
                {
                    CustomerID = model.CustomerID,
                    OrderDate = model.OrderDate,
                    Status = model.Status,
                    OrderProducts = selectedProducts.Select(p => new OrderProduct
                    {
                        ProductID = p.ProductID,
                        Quantity = p.Quantity,
                        Price = p.Price
                    }).ToList()
                };
                // Calculate Total Amount based on selected products
                newOrder.TotalAmount = selectedProducts.Sum(sp => sp.Price * sp.Quantity);

                // Add the order to the repository and save changes
                _orderRepo.Add(newOrder);
                _orderRepo.save();

                return RedirectToAction("Index");
            }

            // If we reach here, there was a validation error
            return View(model);
        }



        // Render form for editing an existing order
        // Render form for editing an existing order
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                // Return a NotFound view with an appropriate error message
                ModelState.AddModelError(string.Empty, "Order not found.");
                return View("NotFound"); // You can create a NotFound view or handle it differently
            }

            var orderProducts = order.OrderProducts ?? new List<OrderProduct>();

            var model = new EditOrderViewModel
            {
                OrderID = order.OrderID,
                CustomerID = order.CustomerID,
                OrderDate = order.OrderDate,
                Status = order.Status,
                SelectedProducts = orderProducts.Select(op => new OrderProductViewModel
                {
                    ProductID = op.ProductID,
                    ProductName = op.Product?.Name ?? "Unknown",
                    Quantity = op.Quantity,
                    Price = op.Price
                }).ToList(),
                Customers = _customerRepo.GetALL()?.ToList() ?? new List<Customer>(),
                Products = _productRepo.GetALL()?.ToList() ?? new List<Product>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepo.GetById(model.OrderID);
                if (order == null)
                {
                    ModelState.AddModelError(string.Empty, "Order not found.");
                    return View(model);
                }

                // Check for valid ProductIDs and prepare the order products
                var selectedProducts = new List<OrderProduct>();
                foreach (var product in model.SelectedProducts)
                {
                    if (product.IsSelected) // Only consider selected products
                    {
                        if (!_productRepo.Exists(product.ProductID))
                        {
                            ModelState.AddModelError(string.Empty, $"Product with ID {product.ProductID} does not exist.");
                            return View(model);
                        }

                        // Create a new OrderProduct based on the selected product
                        selectedProducts.Add(new OrderProduct
                        {
                            ProductID = product.ProductID,
                            Quantity = product.Quantity,
                            Price = product.Price
                        });
                    }
                }

                // Update order properties
                order.CustomerID = model.CustomerID;
                order.OrderDate = model.OrderDate;
                order.Status = model.Status;

                // Clear existing products and add updated ones
                order.OrderProducts.Clear(); // Assuming OrderProducts is a collection in your Order entity
                order.OrderProducts.AddRange(selectedProducts);

                // Save changes to the repository
                _orderRepo.Update(order);
                _orderRepo.save(); // Ensure this method persists changes to the database

                return RedirectToAction("Index");
            }

            // Repopulate the model with customers and products if ModelState is invalid
            model.Customers = _customerRepo.GetALL()?.ToList() ?? new List<Customer>();
            model.Products = _productRepo.GetALL()?.ToList() ?? new List<Product>();
            return View(model);
        }


        // Delete an order
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
            {
                return NotFound();
            }

            _orderRepo.delete(id);
            _orderRepo.save();

            return RedirectToAction("Index");
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
