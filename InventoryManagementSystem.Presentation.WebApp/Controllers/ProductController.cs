using Inventory.Business.App;
using Inventory.Model.APP;
using Inventory.Presentaion.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Presentaion.App.Controllers
{
    public class ProductController : Controller
    {
        public IProductRepository _repo { get; set; }
        public IRepositry<Supplier> _supplierRepo { get; set; }

        public ProductController(IProductRepository repo, IRepositry<Supplier> supplierRepo)
        {
            _repo = repo;
            _supplierRepo = supplierRepo;
        }

        // Display all products
        public IActionResult Index()
        {
            // Fetch all products from the repository
            var products = _repo.GetProductsWithSuppliers()
                .Select(p => new IndexProductVM
                {
                    ProductID = p.ProductID,
                    Name = p.Name,
                    Category = p.Category,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    SupplierName = p.Supplier != null ? p.Supplier.SupplierName : "No Supplier"
                }).ToList();

            // Pass the product list to the view
            return View(products);
        }
        // Delete product
        public IActionResult Delete([FromQuery] int productID)
        {
            Product product = _repo.GetById(productID);
            if (product == null) return NotFound();

            _repo.delete(productID);
            _repo.save();
            return RedirectToAction("Index");
        }
        // View product details
        public IActionResult Details([FromQuery] int productID)
        {
            // Use Include to load the related Supplier entity
            Product product = _repo.GetProductsWithSuppliers().FirstOrDefault(p => p.ProductID == productID);

            if (product == null)
            {
                return NotFound();
            }

            // Create a view model for the supplier details
            IndexProductVM model = new IndexProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Category = product.Category,
                SupplierName = product.Supplier?.SupplierName, // Safely access SupplierName
                SupplierID = product.Supplier?.SupplierID ?? 0 // Safely access SupplierID
            };

            return View(model);
        }
        // Restock a product
        [HttpGet]
        public IActionResult Restock([FromQuery] int productID)
        {
            Product product = _repo.GetById(productID);
            if (product == null) return NotFound();

            // Store ProductID in TempData
            TempData["ProductID"] = product.ProductID;

            IndexProductVM model = new IndexProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Quantity = product.Quantity
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveRestock(int restockQuantity)
        {
            // Retrieve ProductID from TempData
            if (TempData["ProductID"] == null)
            {
                return BadRequest("Product ID is missing.");
            }

            int productID = (int)TempData["ProductID"];
            Product product = _repo.GetById(productID);
            if (product == null) return NotFound();

            product.Quantity += restockQuantity;
            _repo.Update(product);
            _repo.save();

            return RedirectToAction("Index");
        }


        // Form to edit an existing product
        
        [HttpGet]
        public IActionResult Edit(int productID)
        {
            // Fetch the product by ID
            var product = _repo.GetById(productID);

            if (product == null)
            {
                return NotFound();
            }
            // Check if the product's Supplier is null
            var supplierName = product.Supplier != null ? product.Supplier.SupplierName : "No Supplier";
            // Map the product to the view model
            IndexProductVM model = new IndexProductVM
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Category = product.Category,
                Quantity = product.Quantity,
                Price = product.Price,
                SupplierID = product.SupplierID,
                SupplierName = supplierName

            };

            // Populate the suppliers dropdown
            ViewBag.Suppliers = new SelectList(_supplierRepo.GetALL(), "SupplierID", "SupplierName", product.SupplierID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IndexProductVM model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the product from the database
                var product = _repo.GetById(model.ProductID);

                if (product == null)
                {
                    return NotFound();
                }

                // Update product properties
                product.Name = model.Name;
                product.Category = model.Category;
                product.Quantity = model.Quantity;
                product.Price = model.Price;
                product.SupplierID = model.SupplierID;

                _repo.Update(product);
                _repo.save();

                return RedirectToAction("Index");
            }

            // Reload suppliers if validation fails
            ViewBag.Suppliers = new SelectList(_supplierRepo.GetALL(), "SupplierID", "SupplierName", model.SupplierID);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Prepare suppliers dropdown data if needed
            ViewBag.Suppliers = new SelectList(_supplierRepo.GetALL(), "SupplierID", "SupplierName");

            return View(new IndexProductVM()); // Use IndexProductVM
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IndexProductVM model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    Name = model.Name,
                    Category = model.Category,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    SupplierID = model.SupplierID
                    

                };
                _repo.Add(product);
                _repo.save();
                return RedirectToAction("Index");
            }
               
                // Reload suppliers if validation fails
               ViewBag.Suppliers = new SelectList(_supplierRepo.GetALL(), "SupplierID", "SupplierName");
               return View("Create", model);

        }
    }
}
