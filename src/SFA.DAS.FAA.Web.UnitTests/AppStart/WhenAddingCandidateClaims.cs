using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.AppStart;

public class WhenAddingCandidateClaims
{
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Candidate(
        string nameIdentifier,
        string emailAddress,
        PutCandidateApiResponse response,
        [Frozen] Mock<IApiClient> apiClient,
        CandidateAccountPostAuthenticationClaimsHandler handler)
    {
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, emailAddress);
        var request = new PutCandidateApiRequest(nameIdentifier, new PutCandidateApiRequestData
        {
            Email = emailAddress
        });
        apiClient.Setup(x =>
                x.Put<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(request.PutUrl)
                                                       && ((PutCandidateApiRequestData)c.Data).Email == emailAddress
                    )))
            .ReturnsAsync(response);
        
        var actual = await handler.GetClaims(tokenValidatedContext);
        
        actual.First(c => c.Type.Equals(ClaimTypes.GivenName)).Value.Should().Be(response.FirstName);
        actual.First(c => c.Type.Equals(ClaimTypes.Surname)).Value.Should().Be(response.LastName);
        actual.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value.Should().Be(response.Id.ToString());
        actual.First(c => c.Type.Equals(CustomClaims.DisplayName)).Value.Should().Be($"{response.FirstName} {response.LastName}");
    }

    [Test, MoqAutoData]
    public async Task Then_The_FirstName_And_LastName_Are_Not_Populated_If_Not_Available(
        string nameIdentifier,
        string emailAddress,
        PutCandidateApiResponse response,
        [Frozen] Mock<IApiClient> apiClient,
        CandidateAccountPostAuthenticationClaimsHandler handler)
    {
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, emailAddress);
        response.FirstName = null;
        response.LastName = null;
        var request = new PutCandidateApiRequest(nameIdentifier, new PutCandidateApiRequestData
        {
            Email = emailAddress
        });
        apiClient.Setup(x =>
                x.Put<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(request.PutUrl)
                                                       && ((PutCandidateApiRequestData)c.Data).Email == emailAddress
                    )))
            .ReturnsAsync(response);
        
        var actual = await handler.GetClaims(tokenValidatedContext);
        
        actual.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GivenName))?.Value.Should().BeNull();
        actual.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname))?.Value.Should().BeNull();
        actual.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value.Should().Be(response.Id.ToString());
        actual.FirstOrDefault(c => c.Type.Equals(CustomClaims.DisplayName))?.Value.Should().BeNull();
    }
    
    private TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string emailAddress)
    {
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
            new Claim(ClaimTypes.Email, emailAddress)
        });
        
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));
        return new TokenValidatedContext(new DefaultHttpContext(), new AuthenticationScheme(",","", typeof(TestAuthHandler)),
            new OpenIdConnectOptions(), Mock.Of<ClaimsPrincipal>(), new AuthenticationProperties())
        {
            Principal = claimsPrincipal
        };
    }
    
    private class TestAuthHandler : IAuthenticationHandler
    {
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }
    }
}