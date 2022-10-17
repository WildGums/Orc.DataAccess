namespace Orc.DataAccess
{
    using Catel;
    using Catel.Data;

    public static class IValidationContextExtensions
    {
        public static void AddValidationError(this IValidationContext validationContext, string message, string? tag = null)
        {
            var businessRuleValidationResult = new BusinessRuleValidationResult(ValidationResultType.Error, message)
            {
                Tag = tag
            };

            validationContext.Add(businessRuleValidationResult);
        }
    }
}
