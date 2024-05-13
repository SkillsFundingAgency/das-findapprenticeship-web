using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using FluentValidation.Results;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies;

public class WhenGettingVacancyDetails
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Browse_By_Interests_View_Returned(
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
        request.VacancyReference = "VAC1000012484";
        mediator.Setup(x => x.Send(It.IsAny<GetApprenticeshipVacancyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult { });

        var actual = await controller.Vacancy(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as VacancyDetailsViewModel;

        var expected = new VacancyDetailsViewModel().MapToViewModel(dateTimeService, result);

        actualModel.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.ClosingDate)
            .Excluding(x => x.CourseLevelMapper));
    }

    [Test]
    [MoqInlineAutoData("1234567890")]
    [MoqInlineAutoData("VAC12345678")]
    [MoqInlineAutoData("ABC12345678")]
    [MoqInlineAutoData("")]
    public async Task Then_The_Mediator_Query_Is_Called_With_BadRequest_NotFound_Returned(
        string vacancyReference,
        IDateTimeService dateTimeService,
        ValidationResult result,
        [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
        var request = new GetVacancyDetailsRequest
        {
            VacancyReference = vacancyReference
        };

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Vacancy(request) as NotFoundResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}