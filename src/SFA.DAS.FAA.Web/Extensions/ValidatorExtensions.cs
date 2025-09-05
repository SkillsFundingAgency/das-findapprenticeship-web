using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.FAA.Web.Extensions;

public static class ValidatorExtensions
{
    public static async Task<ValidationResult> ValidateAndUpdateModelStateAsync<T>(this IValidator<T> validator, T model, ModelStateDictionary modelState)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(modelState);
        }

        return validationResult;
    }
    
}