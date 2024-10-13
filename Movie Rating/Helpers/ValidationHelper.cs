using System;
using System.ComponentModel.DataAnnotations;

namespace Movie_Rating.Helpers
{
    public class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            //Model Validate PersonName
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                obj, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
