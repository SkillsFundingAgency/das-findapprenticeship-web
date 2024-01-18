using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.FAA.Web.AppStart;

public class CandidateAccountPostAuthenticationClaimsHandler : ICustomClaims
{
    public async Task<IEnumerable<Claim>> GetClaims(TokenValidatedContext ctx)
    {
        return new List<Claim>();
    }
}
