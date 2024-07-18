using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Filters;

public class NewFaaUserAccountFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var identity = (ClaimsIdentity)context.HttpContext.User.Identity;

        if (identity.HasClaim(p => p.Type == CustomClaims.CandidateId) && !identity.HasClaim(p => p.Type == CustomClaims.AccountSetupCompleted))
        {
            if (!context.ActionDescriptor.DisplayName.Contains(nameof(UserController), StringComparison.CurrentCultureIgnoreCase) &&
                !context.ActionDescriptor.DisplayName.Contains(nameof(ServiceController), StringComparison.CurrentCultureIgnoreCase) &&
                !context.ActionDescriptor.DisplayName.Contains(nameof(HomeController), StringComparison.CurrentCultureIgnoreCase))
            {
                var service = context.HttpContext.RequestServices.GetService<IApiClient>();
                var email = identity.Claims.FirstOrDefault(c=>c.Type.Equals(ClaimTypes.Email))?.Value;
                var userId = identity.Claims.FirstOrDefault(c=>c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
                var requestData = new PutCandidateApiRequestData
                {
                    Email = email
                };

                var response = await service?.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(userId, requestData));

                if (response.Status != UserStatus.Completed)
                {
                    var requestPath = context.HttpContext.Request.Path;
                    context.Result = new RedirectToRouteResult(RouteNames.CreateAccount, new {returnUrl = requestPath});
                    return;    
                }
            }
        }
        await next();
    }
}