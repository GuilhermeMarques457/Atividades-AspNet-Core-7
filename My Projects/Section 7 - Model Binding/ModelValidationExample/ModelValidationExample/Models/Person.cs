using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidationExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationExample.Models
{
    public class Person : IValidatableObject
    {
        [Required(ErrorMessage = "{0} Nome deve ser inserido")]
        [Display(Name = "Nome do cliente")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} deve estar entre {2} e {1}")]
        [RegularExpression("^[A-Za-z .]*$", ErrorMessage = "{0} deve conter apenas letras, espaços e ponto")]
        public string? Name { get; set; }
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "{0} and {1} devem ser identicos")]
        public string? ConfirmPassword { get; set; }

        [EmailAddress(ErrorMessage = "Email não esta formatado")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Telefone não esta formatado corretamente")]
        [ValidateNever]
        public string? Phone { get; set; }

        [BindNever]
        [Range(0, 999.99, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        public double? Price { get; set; }

       
        [DateValidations(2006, ErrorMessage = "Voce deve ser maior de {0} anos kakakak")]
        public DateTime? DateOfBirth { get; set; }

        public DateTime? FromDate { get; set; }

        [DataRangeValidationAttribute("FromDate", ErrorMessage = "From date should be older than or equal 'To Date'")]
        public DateTime? ToDate { get; set; }

        public int? Age { get; set; }

        public override string ToString()
        {
            return $"Person object - Person name: {Name}, Email: {Email}, Phone: {Phone}, Password: {Password}, ConfimedPassoword: {ConfirmPassword}, Price: {Price}, Date: {DateOfBirth}";
        }

        //It only will check if the object validator is already withou errors
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.HasValue == false || Age.HasValue == false)
            {
                yield return new ValidationResult("Ou date of birth ou age deve ser preenchido", new[] { nameof(Age) });
            }
        }
    }
}
