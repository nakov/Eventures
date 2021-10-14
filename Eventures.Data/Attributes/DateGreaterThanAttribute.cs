using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Eventures.Data.Attributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        public DateGreaterThanAttribute(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        private string DateToCompareToFieldName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            var earlierDateField = validationContext.ObjectType
                .GetProperty(DateToCompareToFieldName)
                .GetValue(validationContext.ObjectInstance, null);

            var laterDate = (DateTime)value;

            if (earlierDateField == null)
            {
                if (laterDate >= DateTime.Now && laterDate < DateTime.Parse("01/01/2100", CultureInfo.InvariantCulture))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Due Date must be in the future and before '01/01/2100'.");
                }
            }
            else
            {
                DateTime earlierDate = (DateTime)earlierDateField;

                if (laterDate > earlierDate)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("End date should be after the start date.");
                }
            }
        }
    }
}
