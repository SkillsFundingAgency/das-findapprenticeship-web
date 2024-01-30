using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Filters;

public class NewFaaUserAccountFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var identity = (ClaimsIdentity)context.HttpContext.User.Identity;

        if (identity.HasClaim(p => p.Type == CustomClaims.CandidateId) && !identity.HasClaim(p => p.Type == CustomClaims.DisplayName))
        {
            if (!context.ActionDescriptor.DisplayName.Contains("UserController", StringComparison.CurrentCultureIgnoreCase) &&
                !context.ActionDescriptor.DisplayName.Contains("ServiceController", StringComparison.CurrentCultureIgnoreCase))
            {
                var service = context.HttpContext.RequestServices.GetService<IApiClient>();
                var email = identity.Claims.FirstOrDefault(c=>c.Type.Equals(ClaimTypes.Email))?.Value;
                var userId = identity.Claims.FirstOrDefault(c=>c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
                var requestData = new PutCandidateApiRequestData
                {
                    Email = email
                };
                var candidate = await service.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(userId, requestData));
                if (string.IsNullOrEmpty(candidate.FirstName))
                {
                    context.Result = new RedirectToRouteResult(RouteNames.CreateAccount, null);
                    return;    
                }
            }
        }
        await next();
    }
}