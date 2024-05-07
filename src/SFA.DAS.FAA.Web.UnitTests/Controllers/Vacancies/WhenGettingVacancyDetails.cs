using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies;

public class WhenGettingVacancyDetails
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Browse_By_Interests_View_Returned(
        Guid candidateId,
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetApprenticeshipVacancyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString())
                }))

            }
        };

        var actual = await controller.Vacancy(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as VacancyDetailsViewModel;

        var expected = new VacancyDetailsViewModel().MapToViewModel(dateTimeService, result);

        actualModel.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.ClosingDate)
            .Excluding(x => x.CourseLevelMapper));
    }
}