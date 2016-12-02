using Test_Solution1.Common.CustomException;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using System;

namespace Test_Solution1.Common.Extension
{
    public abstract class ValidatorBase<T> : AbstractValidator<T>
    {
        public string ConstructMessage(string message, string propertyName)
        {
            return new MessageFormatter().AppendPropertyName(propertyName).BuildMessage(message);
        }

        public string ConstructMessage(string message, string propertyName, params string[] additionalPropertyNames)
        {
            if (additionalPropertyNames == null) throw new ArgumentNullException("additionalPropertyNames");
            var messageFormatter = new MessageFormatter();
            messageFormatter.AppendPropertyName(propertyName);
            messageFormatter.AppendAdditionalArguments(additionalPropertyNames);
            return messageFormatter.BuildMessage(message);
        }

        public override ValidationResult Validate(T instance)
        {
            var result = base.Validate(instance);
            if (!result.IsValid)
                throw new RulesViolationException(result.ToError());
            return result;
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var result = base.Validate(context);
            if (!result.IsValid)
                throw new RulesViolationException(result.ToError());
            return result;
        }
    }
}