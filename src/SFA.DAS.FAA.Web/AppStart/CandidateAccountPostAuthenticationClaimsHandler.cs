using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
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
        //var claims = new List<Claim>();
        //var userId = ctx.Principal.Claims
        //        .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
        //        .Value;
        //var email = ctx.Principal.Claims
        //        .First(c => c.Type.Equals(ClaimTypes.Email))
        //        .Value;
        //var mobile = ctx.Principal.Claims
        //.First(c => c.Type.Equals(ClaimTypes.MobilePhone))
        //.Value;
        //var candidateId = ctx.Principal.Claims
        //    .First(c => c.Type.Equals(CustomClaims.CandidateId))
        //    .Value;

        //var requestData = new PutCandidateApiRequestData()
        //{
        //    Email = email
        //};
        //var candidate = await _apiClient.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(userId, requestData));

        //// add claims

        return new List<Claim>();
    }
}
