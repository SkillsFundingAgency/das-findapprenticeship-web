using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingConfirmAccountDeletion
{
    [Test, MoqAutoData]
    public void Then_View_Is_Returned(
        [Greedy] UserController controller)
    {
        var result = controller.ConfirmAccountDeletion() as ViewResult;
        result.Should().NotBeNull();
    }
}