using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.GovUK.Auth.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Service;

public class WhenGettingAccountDetails
{
    [Test, MoqAutoData]
    public void Then_The_Get_For_Entering_Stub_Auth_Details_Is_Returned_When_Not_Prod(
    string returnUrl,
    [Frozen] Mock<IConfiguration> configuration,
    [Frozen] Mock<IStubAuthenticationService> stubAuthService,
    [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");

        var actual = controller.AccountDetails(returnUrl) as ViewResult;
        var actualModel = actual.Model as StubAuthenticationViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actualModel.ReturnUrl.Should().Be(returnUrl);
        }
    }

    [Test, MoqAutoData]
    public void Then_The_Get_For_Entering_Stub_Auth_Details_Is_Not_Returned_When_Prod(
        string returnUrl,
        [Frozen] Mock<IConfiguration> configuration,
        [Frozen] Mock<IStubAuthenticationService> stubAuthService,
        [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("prd");

        var actual = controller.AccountDetails(returnUrl) as NotFoundResult;

        actual.Should().NotBeNull();
    }
}
