using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests
{
    [TestFixture]
    public class WhenPostingSaveVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_If_Command_Returns_Redirect_Returned(
            Guid candidateId,
            string vacancyReference,
            SaveVacancyCommandResult mediatorResult,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
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
            mediator.Setup(x => x.Send(It.IsAny<SaveVacancyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.SearchResultsSaveVacancy(vacancyReference) as RedirectToRouteResult;

            actual!.RouteName.Should().Be(RouteNames.SearchResults);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Query_With_Redirect_Command_Returns_JsonOk_Returned(
            Guid candidateId,
            string vacancyReference,
            SaveVacancyCommandResult mediatorResult,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
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
            mediator.Setup(x => x.Send(It.IsAny<SaveVacancyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.SearchResultsSaveVacancy(vacancyReference, false) as JsonResult;

            actual!.Value.Should().Be(StatusCodes.Status200OK);
        }
    }
}
