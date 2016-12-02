using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using ValidationFailure = Test_Solution1.Common.Expression.ValidationFailure;

namespace Test_Solution1.Common.Extension
{
    public static class ValidatorExtension
    {
        public static IEnumerable<ValidationFailure> ToError(this ValidationResult validationResult)
        {
            var errors = new List<ValidationFailure>();

            validationResult.Errors.ToList().ForEach(e => MapField(e.PropertyName, e.ErrorMessage, ref errors));
            return errors;
        }

        private static void MapField(string propertyName, string error, ref List<ValidationFailure> errorCollection)
        {
            errorCollection.Add(new ValidationFailure(propertyName, error));
        }
    }
}