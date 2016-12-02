using Test_Solution1.Common.CustomException;
using FluentValidation;
using FluentValidation.TestHelper;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Test_Solution1.TestFramework
{
    public class CustomValidatorTester<T, TValue> where T : class
    {
        private readonly MemberAccessor<T, TValue> _accessor;
        private readonly string _accessorMemberFullName;
        private readonly IValidator<T> _validator;
        private readonly TValue _value;

        public CustomValidatorTester(Expression<Func<T, TValue>> expression, IValidator<T> validator, TValue value)
        {
            _validator = validator;
            _value = value;
            _accessor = expression;
            _accessorMemberFullName = expression.GetAccessorMemberFullName();
        }

        public void ValidateNoError(T instanceToValidate, string ruleSet = null)
        {
            _accessor.Set(instanceToValidate, _value);
            var count = 0;
            try
            {
                _validator.Validate(instanceToValidate, ruleSet: ruleSet);
            }
            catch (RulesViolationException rex)
            {
                // StarstWith() is used because when using FluentValidator's RuleForEach() in validator class, PropertyName is appended with index number within square bracket
                count = rex.Errors.Count(x => x.PropertyName.StartsWith(_accessorMemberFullName));
            }

            if (count > 0)
                throw new ValidationTestException(string.Format("Expected no validation errors for property {0}",
                                                                _accessorMemberFullName));
        }

        public void ValidateError(T instanceToValidate, string ruleSet = null)
        {
            _accessor.Set(instanceToValidate, _value);
            var count = 0;
            try
            {
                _validator.Validate(instanceToValidate, ruleSet: ruleSet);
                throw new Exception("No validation exception encountered");
            }
            catch (RulesViolationException rex)
            {
                // StarstWith() is used because when using FluentValidator's RuleForEach() in validator class, PropertyName is appended with index number within square bracket
                count = rex.Errors.Count(x => x.PropertyName.StartsWith(_accessorMemberFullName));
            }
            if (count == 0)
            {
                throw new ValidationTestException(string.Format("Expected a validation error for property {0}",
                                                                _accessorMemberFullName));
            }
        }
    }
}