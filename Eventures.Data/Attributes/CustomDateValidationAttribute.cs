using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Data.Attributes
{
    public class CustomDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            var dateTime = (DateTime)value;

            if (dateTime >= DateTime.Now && dateTime < DateTime.Parse("01/01/2100", CultureInfo.InvariantCulture))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Due Date must be in the future and before '01/01/2100'.");
            }
        }
    }
}
