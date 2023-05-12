using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace ModelValidationExample.CustomValidators
{
    public class DateValidationsAttribute : ValidationAttribute
    {
        public int MinimumYear { get; set; }
        public DateValidationsAttribute(int minimunYear)
        {
            MinimumYear = minimunYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           
            if (value != null)
            {
                DateTime date = (DateTime)value;
                if (date.Year > MinimumYear)
                {
                    return new ValidationResult (string.Format(ErrorMessage, MinimumYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return null;
        }
    }
}
