using ExerciceEComerce.Models;
using System.ComponentModel.DataAnnotations;

namespace ExerciceEComerce.CustomValidators
{
    public class InvoicePriceValidation : ValidationAttribute
    {

        public List<Product>? products;

        public InvoicePriceValidation(List<Product>? products)
        {
            this.products = products;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            double totalValue = 0;

            products.ForEach(produtos => {
                totalValue += produtos.Price * produtos.Quantity;
            });

            if(totalValue == Convert.ToInt32(value))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
