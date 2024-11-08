using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.ModelBinding;

public class DayMonthYearDateModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var modelName = bindingContext.ModelName;

        var dayPart = $"{modelName}Day";
        var monthPart = $"{modelName}Month";
        var yearPart = $"{modelName}Year";

        var dayValue = bindingContext.ValueProvider.GetValue(dayPart);
        var monthValue = bindingContext.ValueProvider.GetValue(monthPart);
        var yearValue = bindingContext.ValueProvider.GetValue(yearPart);
        var userAttemptedValue = !string.IsNullOrEmpty(dayValue.FirstValue) ||
                                 !string.IsNullOrWhiteSpace(monthValue.FirstValue) ||
                                 !string.IsNullOrWhiteSpace(yearValue.FirstValue);

        if (!userAttemptedValue)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.Add(dayPart, dayValue, ModelValidationState.Valid);
        bindingContext.ModelState.Add(monthPart, monthValue, ModelValidationState.Valid);
        bindingContext.ModelState.Add(yearPart, yearValue, ModelValidationState.Valid);

        CheckIfDateIsIncomplete(dayValue, monthValue, yearValue, bindingContext, modelName);
        CheckIfDayIsValid(dayValue, bindingContext, modelName);
        CheckIfMonthIsValid(monthValue, bindingContext, modelName);
        CheckIfYearIsValid(yearValue, bindingContext, modelName);

        if (DateTime.TryParse($"{yearValue}-{monthValue}-{dayValue}", out var parsedValue))
        {
            var newModel = new DayMonthYearDate(parsedValue);
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
        var errorMessage = DayMonthYearDateModelErrors.InvalidDate;

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

    private static void CheckIfDateIsIncomplete(ValueProviderResult dayValue, ValueProviderResult monthValue, ValueProviderResult yearValue, ModelBindingContext bindingContext, string modelName)
    {
        if (!string.IsNullOrEmpty(dayValue.FirstValue) && string.IsNullOrEmpty(monthValue.FirstValue) && string.IsNullOrEmpty(yearValue.FirstValue))
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.DateOfBirthIncomplete);
        }
        else if (string.IsNullOrEmpty(dayValue.FirstValue) && !string.IsNullOrEmpty(monthValue.FirstValue) && string.IsNullOrEmpty(yearValue.FirstValue))
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.DateOfBirthIncomplete);
        }
        else if (string.IsNullOrEmpty(dayValue.FirstValue) && string.IsNullOrEmpty(monthValue.FirstValue) && !string.IsNullOrEmpty(yearValue.FirstValue))
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.DateOfBirthIncomplete);
        }
        else if (!string.IsNullOrEmpty(dayValue.FirstValue) && !string.IsNullOrEmpty(monthValue.FirstValue) && string.IsNullOrEmpty(yearValue.FirstValue))
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.DateOfBirthIncomplete);
        }
        else if (string.IsNullOrEmpty(dayValue.FirstValue) && !string.IsNullOrEmpty(monthValue.FirstValue) && !string.IsNullOrEmpty(yearValue.FirstValue))
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.DateOfBirthIncomplete);
        }
        else if (!string.IsNullOrEmpty(dayValue.FirstValue) && string.IsNullOrEmpty(monthValue.FirstValue) && !string.IsNullOrEmpty(yearValue.FirstValue))
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.DateOfBirthIncomplete);
        }
    }

    private static void CheckIfDayIsValid(ValueProviderResult dayValue, ModelBindingContext bindingContext, string modelName)
    {
        if (dayValue.FirstValue.ToString().Length > 2)
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.InvalidDay);
        }
    }

    private static void CheckIfMonthIsValid(ValueProviderResult monthValue, ModelBindingContext bindingContext, string modelName)
    {
        if (monthValue.FirstValue.ToString().Length > 2)
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.InvalidMonth);
        }
    }
    private static void CheckIfYearIsValid(ValueProviderResult yearValue, ModelBindingContext bindingContext, string modelName)
    {
        if (yearValue.FirstValue.ToString().Length != 4)
        {
            bindingContext.ModelState.AddModelError(modelName, DayMonthYearDateModelErrors.InvalidYear);
        }
    }
}
