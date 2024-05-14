using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions;

public class WhenGettingUserClaims
{
    [Test, AutoData]
    public void Then_If_The_Claim_Exists_Then_Returned(Guid candidateId)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString())
        }));

        var actual = user.Claims.CandidateId();

        actual.Should().Be(candidateId);
    }
    
    [Test, AutoData]
    public void Then_If_The_Claim_Exists_But_Wrong_Format_Then_Empty_Guid_Returned(string candidateId)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId)
        }));

        var actual = user.Claims.CandidateId();

        actual.Should().BeNull();
    }
    
    [Test]
    public void Then_If_The_Claim_Does_Not_Exist_Then_Returned_Empty_Guid_Returned()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new("otherclaim", "ABC123")
        }));

        var actual = user.Claims.CandidateId();

        actual.Should().BeNull();
    }
    
    [Test, AutoData]
    public void Then_If_The_Email_Claim_Exists_Then_Returned(string email)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Email, email)
        }));

        var actual = user.Claims.Email();

        actual.Should().Be(email);
    }
    
    [Test]
    public void Then_If_No_Email_Claim_Exists_Then_Null_Returned()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        }));

        var actual = user.Claims.Email();

        actual.Should().BeNull();
    }
    
    [Test, AutoData]
    public void Then_If_The_NameIdentifier_Claim_Exists_Then_Returned(string email)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, email)
        }));

        var actual = user.Claims.GovIdentifier();

        actual.Should().Be(email);
    }
    
    [Test]
    public void Then_If_No_NameIdentifier_Claim_Exists_Then_Null_Returned()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        }));

        var actual = user.Claims.GovIdentifier();

        actual.Should().BeNull();
    }
}