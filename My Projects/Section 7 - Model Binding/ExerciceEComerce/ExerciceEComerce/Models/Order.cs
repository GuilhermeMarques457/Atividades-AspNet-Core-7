using System.ComponentModel.DataAnnotations;
using ExerciceEComerce.CustomValidators;

namespace ExerciceEComerce.Models
{
    public class Order
    {

        //[Required(ErrorMessage = "{0} is required")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public decimal? TotalPrice { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public List<Product>? Products { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        public decimal? InvoicePrice { get; set; }
            

        [DateValidator()]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime? OrderDate { get; set; }

       

    }
}
