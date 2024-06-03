using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Applications.LegacyApplications;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenPostingConfirmTransfer
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            string email,
            Guid candidateId,
            ConfirmTransferViewModel viewModel,
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
                        new Claim(ClaimTypes.Email, email)
                    }))
                }
            };

            mediator.Setup(x => x.Send(It.Is<MigrateLegacyApplicationsCommand>(x => x.CandidateId == candidateId && x.EmailAddress == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var result = await controller.ConfirmDataTransfer(viewModel) as RedirectToRouteResult;
            result!.RouteName.Should().Be(RouteNames.FinishAccountSetup);
        }
    }
}
