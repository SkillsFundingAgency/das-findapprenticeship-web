using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Filters;

public class SignInLinkFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller controller)
        {
            return;
        }

        if (controller.User.Identity is { IsAuthenticated: true })
        {
            controller.ViewBag.SignInValue = "";
            base.OnActionExecuting(context);
            return;
        }
        
        
        var service = context.HttpContext.RequestServices.GetService<IDataProtectorService>();

        var dataToEncode =
            $"{controller.RouteData.Values["controller"]}|{controller.RouteData.Values["action"]}|{Guid.NewGuid()}";
        
        
        controller.ViewBag.SignInValue = service.EncodedData(dataToEncode);

        base.OnActionExecuting(context);
    }
}