using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Service;
public class WhenSigningOut
{
    [Test, MoqAutoData]
    public void Then_The_View_Is_Returned_With_Model(
    [Frozen] Mock<IConfiguration> configuration,
    [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");

        var actual = controller.SignedOut() as ViewResult;
        var actualModel = actual?.Model as SignedOutViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actualModel.ServiceLink.Should().BeEquivalentTo("https://test-findapprenticeship.apprenticeships.education.gov.uk");

        }
    }
}
