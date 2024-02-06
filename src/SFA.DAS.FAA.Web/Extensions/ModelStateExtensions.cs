using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void Add(this ModelStateDictionary modelState, string key, ValueProviderResult value, ModelValidationState validationState)
        {
            modelState.SetModelValue(key, value);
            modelState[key].ValidationState = validationState;
        }
    }
}
