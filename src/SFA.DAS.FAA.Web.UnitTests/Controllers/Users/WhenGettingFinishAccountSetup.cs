using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

[TestFixture]
public class WhenGettingFinishAccountSetup
{
    [Test, MoqAutoData]
    public void Then_View_Is_Returned(
        [Greedy] UserController controller)
    {
        var result = controller.FinishAccountSetup();

        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }
}