
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenGettingCreateAccount
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        Guid candidateId,
        GetInformQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                }))

            }
        };
        queryResult.IsAccountCreated = false;
        mediator.Setup(x => x.Send(It.Is<GetInformQuery>(r => r.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.CreateAccount(string.Empty);

        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_If_Account_Is_Already_Created_Redirect_To_Search(
        Guid candidateId,
        GetInformQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                }))

            }
        };
        queryResult.IsAccountCreated = true;
        mediator.Setup(x => x.Send(It.Is<GetInformQuery>(r => r.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);
        
        var result = await controller.CreateAccount(string.Empty) as RedirectToRouteResult;

        result.Should().NotBeNull();
        result!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
}
