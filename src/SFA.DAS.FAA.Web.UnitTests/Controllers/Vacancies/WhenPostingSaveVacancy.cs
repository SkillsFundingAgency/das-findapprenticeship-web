using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies;

[TestFixture]
public class WhenPostingSaveVacancy
{
    [Test, MoqAutoData]
    public async Task Then_If_Command_Returns_Redirect_Returned(
        Guid candidateId,
        string vacancyId,
        SaveVacancyCommandResult mediatorResult,
        IDateTimeService dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacanciesController controller)
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
        mediator.Setup(x => x.Send(It.Is<SaveVacancyCommand>(c =>
                c.VacancyId == vacancyId && c.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);
        var vacancyReference = vacancyId.Split('-')[0];

        var actual = await controller.VacancyDetailsSaveVacancy(vacancyId) as RedirectToRouteResult;

        actual!.RouteName.Should().Be(RouteNames.Vacancies);
        actual.RouteValues.Should().NotBeEmpty();
        actual.RouteValues!["VacancyReference"].Should().Be(vacancyReference);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Query_With_Redirect_Command_Returns_JsonOk_Returned(
        Guid candidateId,
        string vacancyId,
        SaveVacancyCommandResult mediatorResult,
        IDateTimeService dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacanciesController controller)
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
        mediator.Setup(x => x.Send(It.Is<SaveVacancyCommand>(c =>
                c.VacancyId == vacancyId && c.CandidateId == candidateId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var actual = await controller.VacancyDetailsSaveVacancy(vacancyId, false) as JsonResult;

        actual!.Value.Should().Be(StatusCodes.Status200OK);
    }
}