using FluentValidation;
using System;
using System.Linq.Expressions;
using Test_Solution1.TestFramework;

namespace Russell.IPC.Test.Utilities.Validator
{
    public static class ValidationTestExtension
    {
        public static void ShouldThrowAndHaveValidationErrorFor<T, TValue>(this IValidator<T> validator,
                                                                           Expression<Func<T, TValue>> expression,
                                                                           TValue value, string ruleSet = null) where T : class, new()
        {
            new CustomValidatorTester<T, TValue>(expression, validator, value).ValidateError(new T(), ruleSet);
        }

        public static void ShouldThrowAndHaveValidationErrorFor<T, TValue>(this IValidator<T> validator,
                                                                           Expression<Func<T, TValue>> expression,
                                                                           T objectToTest, string ruleSet = null) where T : class
        {
            var value = expression.Compile()(objectToTest);
            new CustomValidatorTester<T, TValue>(expression, validator, value).ValidateError(objectToTest, ruleSet);
        }

        public static void ShouldNotThrowAndHaveValidationErrorFor<T, TValue>(this IValidator<T> validator,
                                                                              Expression<Func<T, TValue>> expression,
                                                                              TValue value, string ruleSet = null) where T : class, new()
        {
            new CustomValidatorTester<T, TValue>(expression, validator, value).ValidateNoError(new T(), ruleSet);
        }

        public static void ShouldNotThrowAndHaveValidationErrorFor<T, TValue>(this IValidator<T> validator,
                                                                              Expression<Func<T, TValue>> expression,
                                                                              T objectToTest, string ruleSet = null) where T : class
        {
            var value = expression.Compile()(objectToTest);
            new CustomValidatorTester<T, TValue>(expression, validator, value).ValidateNoError(objectToTest, ruleSet);
        }
    }
}