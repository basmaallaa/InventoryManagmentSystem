using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Inventory.Presentaion.App.ViewModel
{
    public class IndexProductVM
    {
        [IntegerValidator]
        public int ProductID { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z]{2,}\\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\\s?([a-zA-Z]{1,})?)", ErrorMessage = "Valid Charactors include (A-Z) (a-z) (' space -)")]
        public string Name { get; set; }

        [Required]
        [IntegerValidator]
        public int Quantity { get; set; }
        public string? Category {  get; set; }
        [Required]
        public double Price {  get; set; }

       
        public string? SupplierName {  get; set; }

        

        public int SupplierID { get; set; }
    }
}
