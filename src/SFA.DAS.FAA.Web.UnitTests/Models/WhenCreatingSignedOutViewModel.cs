using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
public class WhenCreatingSignedOutViewModel
{
    [TestCase("at", "at-findapprenticeship.apprenticeships.education")]
    [TestCase("test", "test-findapprenticeship.apprenticeships.education")]
    [TestCase("test2", "test2-findapprenticeship.apprenticeships.education")]
    [TestCase("pp", "pp-findapprenticeship.apprenticeships.education")]
    [TestCase("Mo", "mo-findapprenticeship.apprenticeships.education")]
    [TestCase("Demo", "demo-findapprenticeship.apprenticeships.education")]
    [TestCase("prd", "findapprenticeship.education")]
    public void Then_The_Url_Is_Built_Correctly_For_Each_Environment(string environment, string expectedUrlPart)
    {
        var model = new SignedOutViewModel(environment);

        model.ServiceLink.Should().BeEquivalentTo($"https://{expectedUrlPart}.gov.uk");
    }
}
