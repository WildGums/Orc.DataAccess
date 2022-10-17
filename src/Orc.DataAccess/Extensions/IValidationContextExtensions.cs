namespace Orc.DataAccess
{
    using Catel;
    using Catel.Data;

    public static class IValidationContextExtensions
    {
        #region Methods
        public static void AddValidationError(this IValidationContext validationContext, string message, string tag = null)
        {
            Argument.IsNotNull(() => validationContext);

            var businessRuleValidationResult = new BusinessRuleValidationResult(ValidationResultType.Error, message)
            {
                Tag = tag
            };

            validationContext.Add(businessRuleValidationResult);
        }
        #endregion
    }
}
