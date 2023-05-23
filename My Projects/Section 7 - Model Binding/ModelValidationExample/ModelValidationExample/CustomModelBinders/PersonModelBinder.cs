using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.Models;

namespace ModelValidationExample.CustomModelBinders
{
    public class PersonModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Person person = new Person();

            if (bindingContext.ValueProvider.GetValue("FirstName").Length > 0)
            {
                person.Name = bindingContext.ValueProvider.GetValue("FirstName").FirstValue;
            };

            if (bindingContext.ValueProvider.GetValue("LastName").Length > 0)
            {
                person.Name += " " + bindingContext.ValueProvider.GetValue("LastName").FirstValue;
            };

            if (bindingContext.ValueProvider.GetValue("Email").Length > 0)
            {
                person.Email = bindingContext.ValueProvider.GetValue("Email").FirstValue;
            };

            if (bindingContext.ValueProvider.GetValue("Phone").Length > 0)
            {
                person.Phone = bindingContext.ValueProvider.GetValue("Phone").FirstValue;
            };

            if (bindingContext.ValueProvider.GetValue("DateOfBirth").Length > 0)
            {
                person.DateOfBirth = Convert.ToDateTime(bindingContext.ValueProvider.GetValue("DateOfBirth").FirstValue);
            };

            if (bindingContext.ValueProvider.GetValue("Age").Length > 0)
            {
                person.Age = int.Parse(bindingContext.ValueProvider.GetValue("Age").FirstValue);
            };

            bindingContext.Result = ModelBindingResult.Success(person);
            return Task.CompletedTask;


        }
    }
}
