using Inventory.Business.App;
using Inventory.Model.APP;
using Inventory.Presentaion.App.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Presentaion.App.Controllers
{
    public class SupplierController : Controller
    {
        public IRepositry<Supplier> _repo { get; set; }

        public SupplierController(IRepositry<Supplier> repo)
        {
            _repo = repo;

        }
        
        [HttpGet]
        public IActionResult Index()
        {
            List<IndexSupplierVM> model = new List<IndexSupplierVM>();
            ICollection<Supplier> suppliers = _repo.GetALL().ToList();
            foreach (Supplier s in suppliers)
            {

                IndexSupplierVM item = new IndexSupplierVM();
                item.SupplierName = s.SupplierName;
                item.SupplierID = s.SupplierID;
                item.ContactInfo = s.ContactInfo;
                model.Add(item);
            }

            return View(model);
        }
        // Render form to add supplier
        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSupplier(IndexSupplierVM model)
        {
            if (ModelState.IsValid)
            {
                Supplier supplier = new Supplier();

                supplier.SupplierName = model.SupplierName;
                supplier.ContactInfo = model.ContactInfo;
                supplier.SupplierID = model.SupplierID;
                _repo.Add(supplier);
                _repo.save();
                return RedirectToAction("Index");
            }
            else
            {
                return View("AddSupplier", model);
            }
        }

        // Update supplier details
        [HttpGet]
        public IActionResult UpdateSupplier([FromQuery] int supplierID)
        {
            Supplier supplier = _repo.GetById(supplierID);
            IndexSupplierVM indexSupplierVM = new IndexSupplierVM();
            indexSupplierVM.SupplierName = supplier.SupplierName;
            indexSupplierVM.ContactInfo = supplier.ContactInfo;
            indexSupplierVM.SupplierID = supplier.SupplierID;


            return View(indexSupplierVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSupplier(IndexSupplierVM supplier, [FromQuery] int supplierID)
        {
            if (supplier == null)
            {
                return NotFound(); // or handle the error appropriately
            }


            Supplier oldSupplier = _repo.GetById(supplier.SupplierID);
            if (oldSupplier == null)
            {
                return NotFound(); // Handle case where supplier does not exist
            }

            oldSupplier.SupplierName = supplier.SupplierName;
            oldSupplier.ContactInfo = supplier.ContactInfo;
            _repo.Update(oldSupplier);
            _repo.save();

            return RedirectToAction("Index");
        }

        // Delete supplier
        public IActionResult DeleteSupplier([FromQuery] int supplierID)
        {
            _repo.delete(supplierID);
            _repo.save();

            return RedirectToAction("Index");
        }
        // Action to display detailed information about a supplier
        [HttpGet]
        public IActionResult DetailsSupplier([FromQuery] int supplierID)
        {
            Supplier supplier = _repo.GetById(supplierID);

            if (supplier == null)
            {
                return NotFound();
            }

            // Create a view model for the supplier details
            IndexSupplierVM model = new IndexSupplierVM
            {
                SupplierID = supplier.SupplierID,
                SupplierName = supplier.SupplierName,
                ContactInfo = supplier.ContactInfo
            };

            return View(model);
        }
    }
}
