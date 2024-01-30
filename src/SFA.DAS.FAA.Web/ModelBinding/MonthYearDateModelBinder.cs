using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.ModelBinding
{
    public class MonthYearDateModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var monthPart = $"{modelName}Month";
            var yearPart = $"{modelName}Year";

            var monthValue = bindingContext.ValueProvider.GetValue(monthPart);

            if (monthValue == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var yearValue = bindingContext.ValueProvider.GetValue(yearPart);

            if (yearValue == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(monthPart, monthValue);
            bindingContext.ModelState.SetModelValue(yearPart, yearValue);
            bindingContext.ModelState[monthPart].ValidationState = ModelValidationState.Valid;
            bindingContext.ModelState[yearPart].ValidationState = ModelValidationState.Valid;

            var fullValue = $"{yearValue}-{monthValue}-01";
            if (DateTime.TryParse(fullValue, out var parsedValue))
            {
                var newModel = new MonthYearDate(fullValue);
                newModel.DateTimeValue = parsedValue;
                bindingContext.Model = newModel;

                bindingContext.Result = ModelBindingResult.Success(newModel);

                var result = new ValueProviderResult(parsedValue.ToString());
                bindingContext.ModelState.SetModelValue(modelName, result);

                return Task.CompletedTask;
            }

            if(!string.IsNullOrWhiteSpace(monthValue.ToString()) || !string.IsNullOrWhiteSpace(yearValue.ToString()))
            {
                var errorMessage = "Enter a real date";

                var holderType = bindingContext.ModelMetadata.ContainerType;
                if (holderType != null)
                {
                    var propertyType = holderType.GetProperty(bindingContext.ModelMetadata.PropertyName);
                    var attributes = propertyType.GetCustomAttributes(true);
                    var hasAttribute = attributes
                        .Cast<Attribute>()
                        .Any(a => a.GetType().IsEquivalentTo(typeof(ModelBindingErrorAttribute)));
                    if (hasAttribute)
                    {
                        var att = (ModelBindingErrorAttribute) attributes.First(x => x.GetType().IsEquivalentTo(typeof(ModelBindingErrorAttribute)));
                        errorMessage = att.ErrorMessage;
                    }
                }

                bindingContext.ModelState.AddModelError(modelName, errorMessage);
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}
