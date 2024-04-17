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
        var userId = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.NameIdentifier))
            .Value;
        
        var email = ctx.Principal.Claims
            .First(c => c.Type.Equals(ClaimTypes.Email))
            .Value;
        var requestData = new PutCandidateApiRequestData()
        {
            Email = email
        };
        //var candidate = await _apiClient.Put<PutCandidateApiResponse>(new PutCandidateApiRequest(userId, requestData));

        var candidate = new PutCandidateApiResponse
        {
            Id = Guid.Parse("A7982B5E-4791-48C3-F5F6-08DC1E861F30"),
            FirstName = "Balaji",
            LastName = "Jambulingam",
            Email = email,
            GovUkIdentifier = "897f2117-8c01-4417-96ff-372c03ea499a"
        };

        // add claims

        var claims = new List<Claim>{new Claim(CustomClaims.CandidateId, candidate.Id.ToString())};

        if (!string.IsNullOrEmpty(candidate.FirstName) && !string.IsNullOrEmpty(candidate.LastName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, candidate.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, candidate.LastName));
            claims.Add(new Claim(CustomClaims.DisplayName, $"{candidate.FirstName} {candidate.LastName}"));
        }
        
        return claims;
    }
}
