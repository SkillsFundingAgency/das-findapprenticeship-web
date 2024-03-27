using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.ModelBinding;

public class DayMonthYearDateModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(DayMonthYearDate))
        {
            return new BinderTypeModelBinder(typeof(DayMonthYearDateModelBinder));
        }

        return null;
    }
}
