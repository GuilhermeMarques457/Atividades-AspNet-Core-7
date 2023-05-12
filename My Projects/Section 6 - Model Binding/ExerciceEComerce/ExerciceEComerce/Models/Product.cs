using System.ComponentModel.DataAnnotations;

namespace ExerciceEComerce.Models
{
    public class Product
    {
        [Required(ErrorMessage = "{0} is required")]
        public int ProductCode;

        [Required(ErrorMessage = "{0} is required")]
        public double Price;

        [Required(ErrorMessage = "{0} is required")]
        public int Quantity;
    }
}
