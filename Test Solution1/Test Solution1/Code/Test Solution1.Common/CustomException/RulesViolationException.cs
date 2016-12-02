using Test_Solution1.Common.Expression;
using System;
using System.Collections.Generic;

namespace Test_Solution1.Common.CustomException
{
    public class RulesViolationException : Exception
    {
        public RulesViolationException(IEnumerable<ValidationFailure> error)
        {
            Errors = error;
        }

        public RulesViolationException(string errorMessage)
        {
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("General", errorMessage) };
            Errors = validationFailures;
        }

        public IEnumerable<ValidationFailure> Errors { get; set; }
    }
}