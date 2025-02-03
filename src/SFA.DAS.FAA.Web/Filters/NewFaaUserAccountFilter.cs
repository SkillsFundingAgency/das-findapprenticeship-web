using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Attributes;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Filters;

public class NewFaaUserAccountFilter : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var attributes = descriptor.MethodInfo.CustomAttributes;
        var skipNewFaaUserAccountCheck = attributes.Any(a => a.AttributeType == typeof(SkipNewFaaUserAccountFilter));

        var identity = (ClaimsIdentity)context.HttpContext.User.Identity;

        if (!skipNewFaaUserAccountCheck && identity != null && identity.HasClaim(p => p.Type == CustomClaims.CandidateId) && !identity.HasClaim(p => p.Type == CustomClaims.AccountSetupCompleted))
        {
            if (!context.ActionDescriptor.HasAttribute<AllowIncompleteAccountAccessAttribute>())
            {
                var response = await GetCandidateDetails(context, identity);
                if (response.Status is UserStatus.Incomplete or UserStatus.InProgress)
                {
                    var requestPath = context.HttpContext.Request.Path;
                    context.Result = new RedirectToRouteResult(RouteNames.CreateAccount, new { returnUrl = requestPath });
                    return;
                }
            }
        }

        if (!skipNewFaaUserAccountCheck && identity != null && identity.HasClaim(c => c.Type == ClaimTypes.Email) && identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
        {
            var response = await GetCandidateDetails(context, identity);
            ((Controller)context.Controller).ViewData["IsAccountStatusCompleted"] = response.Status switch
            {
                UserStatus.Completed => true,
                _ => false
            };
        }

        await next();
    }

    private static async Task<PutCandidateApiResponse> GetCandidateDetails(ActionContext context, ClaimsIdentity identity)
    {
        var service = context.HttpContext.RequestServices.GetService<IApiClient>();
        var email = identity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;
        var userId = identity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        var result = await service!.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(userId!, new PutCandidateApiRequestData { Email = email! }));
        return result;
    }
}