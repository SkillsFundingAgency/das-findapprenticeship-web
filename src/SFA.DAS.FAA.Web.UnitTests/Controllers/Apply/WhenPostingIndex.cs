using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;

public class WhenPostingIndex
{
    [Test, MoqAutoData]
    public async Task Then_RedirectToRoute_Returned(
        Guid applicationId,
        [Greedy] ApplyController controller)
    {
        var actual = controller.Index(applicationId) as RedirectToRouteResult;

        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.ApplicationSubmitted);
    }
}
