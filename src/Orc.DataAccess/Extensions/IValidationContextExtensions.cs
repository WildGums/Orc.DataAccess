namespace Orc.DataAccess
{
    using System;
    using Catel.Data;

    public static class IValidationContextExtensions
    {
        public static void AddValidationError(this IValidationContext validationContext, string message, string? tag = null)
        {
            ArgumentNullException.ThrowIfNull(validationContext);

            var businessRuleValidationResult = new BusinessRuleValidationResult(ValidationResultType.Error, message)
            {
                Tag = tag
            };

            validationContext.Add(businessRuleValidationResult);
        }
    }
}
