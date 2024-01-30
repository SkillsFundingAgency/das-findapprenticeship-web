using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.ModelBinding
{
    public class MonthYearDateModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var modelName = bindingContext.ModelName;

            var monthPart = $"{modelName}Month";
            var yearPart = $"{modelName}Year";

            var monthValue = bindingContext.ValueProvider.GetValue(monthPart);
            var yearValue = bindingContext.ValueProvider.GetValue(yearPart);
            var userAttemptedValue = !string.IsNullOrWhiteSpace(monthValue.FirstValue) ||
                                     !string.IsNullOrWhiteSpace(yearValue.FirstValue);

            if (!userAttemptedValue)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.Add(monthPart, monthValue, ModelValidationState.Valid);
            bindingContext.ModelState.Add(yearPart, yearValue, ModelValidationState.Valid);

            if (DateTime.TryParse($"{yearValue}-{monthValue}-01", out var parsedValue))
            {
                var newModel = new MonthYearDate(parsedValue);
                bindingContext.Model = newModel;

                bindingContext.Result = ModelBindingResult.Success(newModel);

                var result = new ValueProviderResult(parsedValue.ToString());
                bindingContext.ModelState.SetModelValue(modelName, result);

                return Task.CompletedTask;
            }

            bindingContext.ModelState.AddModelError(modelName, GetErrorMessage(bindingContext));
            bindingContext.Result = ModelBindingResult.Failed();

            return Task.CompletedTask;
        }

        private static string GetErrorMessage(ModelBindingContext bindingContext)
        {
            var errorMessage = "Enter a real date";

            var container = bindingContext.ModelMetadata.ContainerType;
            if (container == null) return errorMessage;
            var propertyType = container.GetProperty(bindingContext.ModelMetadata.PropertyName);
            var bindingErrorAttribute = propertyType.GetAttribute<ModelBindingErrorAttribute>();

            if (bindingErrorAttribute != null)
            {
                errorMessage = bindingErrorAttribute.ErrorMessage;
            }

            return errorMessage;
        }
    }
}
