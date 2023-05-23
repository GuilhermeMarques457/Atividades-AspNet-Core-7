using System.ComponentModel.DataAnnotations;

namespace ExerciceEComerce.CustomValidators
{
    public class DateValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value !=null)
            { 
                DateTime date = (DateTime)value;
                if(date.Year < 2000)
                {
                    return new ValidationResult(ErrorMessage);
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
