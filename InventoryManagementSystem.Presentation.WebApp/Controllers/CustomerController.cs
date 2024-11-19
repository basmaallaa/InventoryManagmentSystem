using Inventory.Business.App;
using Inventory.Model.APP;
using Inventory.Presentaion.App.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Presentaion.App.Controllers
{
    public class CustomerController : Controller
    {
        public IRepositry<Customer> _repo { get; set; }

        public CustomerController(IRepositry<Customer> repo)
        {
            _repo = repo;

        }
        //display customers
        [HttpGet]
        public IActionResult Index()
        {
            List<IndexCustomerVM> model = new List<IndexCustomerVM>();
            ICollection<Customer> customers = _repo.GetALL().ToList();
            foreach (Customer customer in customers)
            {

                IndexCustomerVM item = new IndexCustomerVM();
                item.CustomerID = customer.CustomerID;
                item.Name = customer.Name;
                item.Email = customer.Email;
                item.Phone = customer.Phone;
                model.Add(item);
            }

            return View(model);
        }
        //view to render form to add employee
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCustomer(IndexCustomerVM model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                customer.Name = model.Name;
                customer.Email = model.Email;
                customer.Phone = model.Phone;
                _repo.Add(customer);
                _repo.save();
                return RedirectToAction("Index");
            }
            else
            {

                return View("AddCustomer", model);
            }

        }
        [HttpGet]
        public IActionResult Updatecustomer([FromQuery] int customerID)
        {
            Customer customer = _repo.GetById(customerID);
            IndexCustomerVM model = new IndexCustomerVM();
            model.CustomerID = customer.CustomerID; 
            model.Name = customer.Name;
            model.Email = customer.Email;
            model.Phone = customer.Phone;


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Updatecustomer(IndexCustomerVM model, [FromQuery] int customerID)
        
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    return NotFound(); // or handle the error appropriately
                }

                // Get the existing customer from the repository
                Customer Oldcustomer = _repo.GetById(model.CustomerID);
                if (Oldcustomer == null)
                {
                    return NotFound(); // Handle case where customer does not exist
                }


                Oldcustomer.Name = model.Name;
                Oldcustomer.Email = model.Email;
                Oldcustomer.Phone = model.Phone;
                _repo.Update(Oldcustomer);
                _repo.save();

                return RedirectToAction("Index");
            }
            else
            {

                return View("Updatecustomer", model);
            }

        }
        public IActionResult DeleteCustomer([FromQuery] int customerID)
        {
            _repo.delete(customerID);
            _repo.save();

            return RedirectToAction("Index");
        }







    }

    
}
