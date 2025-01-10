using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.FAA.Web.AppStart;

public class CandidateAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    private readonly IApiClient _apiClient;

    public CandidateAccountPostAuthenticationClaimsHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext ctx)
    {
        // Access the HttpContext from the TokenValidatedContext
        var httpContext = ctx.HttpContext;

        // Extract the route information (path)
        if (!string.IsNullOrEmpty(httpContext.Request.Path.Value))
        {
            var path = httpContext.Request.Path.Value?[1..];

            if (!string.IsNullOrEmpty(path) && path.Equals(RouteNames.SignOut, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<Claim>();
            }
        }

        var userId = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
            .Value;
        
        var email = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.Email))
            .Value;

        var requestData = new PutCandidateApiRequestData
        {
            Email = email
        };
        var candidate = await _apiClient.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(userId, requestData));
        
        // add claims

        var claims = new List<Claim>{new Claim(CustomClaims.CandidateId, candidate.Id.ToString())};

        if (!string.IsNullOrEmpty(candidate.FirstName) && !string.IsNullOrEmpty(candidate.LastName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, candidate.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, candidate.LastName));
            claims.Add(new Claim(CustomClaims.DisplayName, $"{candidate.FirstName} {candidate.LastName}"));

            if (candidate.Status == UserStatus.Completed)
            {
                claims.Add(new Claim(CustomClaims.AccountSetupCompleted, "true"));
            }

            if (candidate.PhoneNumber != null)
            {
                claims.Add(new Claim(ClaimTypes.MobilePhone, candidate.PhoneNumber));
            }
        }
        
        return claims;
    }
}
