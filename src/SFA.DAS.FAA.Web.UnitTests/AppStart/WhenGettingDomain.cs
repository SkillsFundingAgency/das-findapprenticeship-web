using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;

namespace SFA.DAS.FAA.Web.UnitTests.AppStart;

public class WhenGettingDomain
{
    
    [TestCase("LocAL","")]
    [TestCase("TEST","test-findapprenticeship.apprenticeships.education.gov.uk")]
    [TestCase("PRD","findapprenticeship.service.gov.uk")]
    [TestCase("prePROD","preprod-findapprenticeship.apprenticeships.education.gov.uk")]
    public void Then_The_Domain_Is_Correct_For_Each_Environment(string environment, string expectedDomain)
    {
        var actual = DomainExtensions.GetDomain(environment);

        actual.Should().Be(expectedDomain);
    }

    
}