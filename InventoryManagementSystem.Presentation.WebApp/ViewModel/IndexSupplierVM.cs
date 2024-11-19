using System.ComponentModel.DataAnnotations;

namespace Inventory.Presentaion.App.ViewModel
{
    public class IndexSupplierVM
    {
        public int SupplierID { get; set; }

        [RegularExpression("^([a-zA-Z]{2,}\\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\\s?([a-zA-Z]{1,})?)", ErrorMessage = "Valid Charactors include (A-Z) (a-z) (' space -)")]
        public string SupplierName { get; set; }
        [Phone]
        [Required]
        public string ContactInfo { get; set; }
    }
}
