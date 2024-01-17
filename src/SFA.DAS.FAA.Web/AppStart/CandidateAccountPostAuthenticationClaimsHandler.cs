using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.FAA.Web.AppStart;

public class CandidateAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext ctx)
    {
        var claims = new List<Claim>();
        var userId = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
            .Value;
        var email = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.Email))
            .Value;
        var phone = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.MobilePhone))
            .Value;

        //todo: upsert users

        return Enumerable.Empty<Claim>();
    }
}
