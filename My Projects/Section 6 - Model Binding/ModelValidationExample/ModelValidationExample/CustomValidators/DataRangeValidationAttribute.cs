using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelValidationExample.CustomValidators
{
    public class DataRangeValidationAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; set; }

        public DataRangeValidationAttribute(string otherPropertyName) 
        {
            OtherPropertyName = otherPropertyName;
        }   

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime toDate = Convert.ToDateTime(value);
                PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

                DateTime fromDate = Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance));

                if (fromDate > toDate)
                {
                    return new ValidationResult(ErrorMessage, new string[] { OtherPropertyName, validationContext.MemberName });
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
