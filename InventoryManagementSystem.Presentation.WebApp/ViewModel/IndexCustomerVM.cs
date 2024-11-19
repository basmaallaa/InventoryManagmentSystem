using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Inventory.Presentaion.App.ViewModel
{
    public class IndexCustomerVM
    {
        [IntegerValidator]
        public int CustomerID { get; set; }

        [RegularExpression("^([a-zA-Z]{2,}\\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\\s?([a-zA-Z]{1,})?)", ErrorMessage = "Valid Charactors include (A-Z) (a-z) (' space -)")]

        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Required]
        public string Phone { get; set; }
    }
}
