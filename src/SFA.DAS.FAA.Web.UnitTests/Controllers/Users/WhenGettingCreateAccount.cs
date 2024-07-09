
using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

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

        mediator.Setup(x => x.Send(It.Is<GetInformQuery>(r => r.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.CreateAccount(string.Empty);

        using var scope = new AssertionScope();
        result.Should().NotBeNull();
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult.Model.Should().NotBeNull();
        var model = viewResult.Model as InformViewModel;
        model.Should().NotBeNull();
        model.ShowAccountRecoveryBanner.Should().Be(queryResult.ShowAccountRecoveryBanner);
    }
}
