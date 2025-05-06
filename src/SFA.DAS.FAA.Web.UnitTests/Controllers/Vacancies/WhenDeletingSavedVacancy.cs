using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies
{
    [TestFixture]
    public class WhenDeletingSavedVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_If_Command_Returns_Redirect_Returned(
            Guid candidateId,
            string vacancyId,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.VacanciesController controller)
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
            var vacancyReference = vacancyId.Split('-')[0];

            var actual = await controller.VacancyDetailsDeleteSavedVacancy(vacancyId, true) as RedirectToRouteResult;

            actual!.RouteName.Should().Be(RouteNames.Vacancies);
            actual.RouteValues.Should().NotBeEmpty();
            actual.RouteValues!["VacancyReference"].Should().Be(vacancyReference);

            mediator.Verify(x => x.Send(It.Is<DeleteSavedVacancyCommand>(c =>
                c.VacancyId == vacancyId && c.CandidateId == candidateId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Command_Returns_JsonOk_Returned(
            Guid candidateId,
            string vacancyId,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.VacanciesController controller)
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
            var vacancyReference = vacancyId.Split('-')[0];

            var actual = await controller.VacancyDetailsDeleteSavedVacancy(vacancyId, false) as JsonResult;

            actual!.Value.Should().Be(StatusCodes.Status200OK);

            mediator.Verify(x => x.Send(It.Is<DeleteSavedVacancyCommand>(c => c.VacancyId == vacancyId && c.CandidateId == candidateId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}