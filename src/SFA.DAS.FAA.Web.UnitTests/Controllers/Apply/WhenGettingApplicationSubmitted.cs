﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;
public class WhenGettingApplicationSubmitted
{
    [Test]
    [MoqInlineAutoData(false, "Do you want to answer equality questions?")]
    [MoqInlineAutoData(true, "Application submitted")]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_Returned(
        bool hasAnsweredEqualityQuestions,
        string expectedPageTitle,
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplyController controller)
    {
        result.HasAnsweredEqualityQuestions = hasAnsweredEqualityQuestions;
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(c =>
                c.CandidateId.Equals(candidateId)
                && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(CustomClaims.CandidateId, candidateId.ToString())
        ]));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var actual = await controller.ApplicationSubmitted(applicationId) as ViewResult;
        var isVacancyClosedEarly = result.ClosedDate.HasValue && result.ClosedDate < result.ClosingDate;

        actual?.Should().NotBeNull();
        var actualModel = actual!.Model as ApplicationSubmittedViewModel;
        actualModel?.ApplicationId.Should().Be(applicationId);
        actualModel!.PageTitle.Should().Be(expectedPageTitle);
        actualModel.VacancyInfo.Should().NotBeNull();
        actualModel.VacancyInfo!.HasAnsweredEqualityQuestions.Should().Be(hasAnsweredEqualityQuestions);
        actualModel.IsVacancyClosed.Should().Be(result.ClosedDate.HasValue || result.ClosingDate < DateTime.UtcNow);
        actualModel.IsVacancyClosedEarly.Should().Be(isVacancyClosedEarly);
        actualModel.ClosedDate.Should().Be(VacancyDetailsHelperService.GetClosedDate(result.ClosedDate, isVacancyClosedEarly));
        mediator.Verify(x => x.Send(It.IsAny<GetApplicationSubmittedQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        mediator.VerifyNoOtherCalls();
    }
}