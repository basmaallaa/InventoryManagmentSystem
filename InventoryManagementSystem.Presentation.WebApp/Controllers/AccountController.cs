using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Presentation.App.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // Define the specific email and password for authentication
            string specificEmail = "admin@gmail.com";
            string specificPassword = "1234";

            // Check if the entered credentials match
            if (email == specificEmail && password == specificPassword)
            {
                // Redirect to a different page (e.g., Home page) upon successful login
                return RedirectToAction("Index", "Home");
            }

            // If credentials are incorrect, show an error message
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }
    }
}
