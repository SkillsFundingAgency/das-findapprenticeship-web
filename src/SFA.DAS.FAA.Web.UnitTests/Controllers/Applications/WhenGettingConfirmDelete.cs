using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Applications.Delete;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications;

public class WhenGettingConfirmDelete
{
    [Test, MoqAutoData]
    public async Task ConfirmDelete_Redirects_To_Index_When_No_Application_Found(
        Guid candidateId,
        Guid applicationId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController sut)
    {
        // arrange
        GetDeleteApplicationQuery? capturedQuery = null;
        sut.AddControllerContext().WithUser(candidateId);
        mediator
            .Setup(x => x.Send(It.IsAny<GetDeleteApplicationQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetDeleteApplicationQueryResult>, CancellationToken>((query, _) => { capturedQuery = query as GetDeleteApplicationQuery; })
            .ReturnsAsync(GetDeleteApplicationQueryResult.None);
        
        // act
        var result = await sut.ConfirmDelete(applicationId, CancellationToken.None) as RedirectToRouteResult;

        // assert
        mediator.Verify(x => x.Send(It.IsAny<GetDeleteApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        result.RouteValues.Should().Contain(x => x.Key == "tab" && x.Value!.ToString() == "Started");
        capturedQuery.Should().NotBeNull();
        capturedQuery.ApplicationId.Should().Be(applicationId);
        capturedQuery.CandidateId.Should().Be(candidateId);
    }
    
    [Test, MoqAutoData]
    public async Task ConfirmDelete_Returns_View(
        Guid candidateId,
        Guid applicationId,
        GetDeleteApplicationQueryResult getDeleteApplicationQueryResult,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController sut)
    {
        // arrange
        getDeleteApplicationQueryResult.EmployerLocationOption = AvailableWhere.MultipleLocations;
        GetDeleteApplicationQuery? capturedQuery = null;
        sut.AddControllerContext().WithUser(candidateId);
        mediator
            .Setup(x => x.Send(It.IsAny<GetDeleteApplicationQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetDeleteApplicationQueryResult>, CancellationToken>((query, _) => { capturedQuery = query as GetDeleteApplicationQuery; })
            .ReturnsAsync(getDeleteApplicationQueryResult);
        
        // act
        var result = await sut.ConfirmDelete(applicationId, CancellationToken.None) as ViewResult;
        var model = result?.Model as ConfirmDeleteApplicationViewModel;

        // assert
        mediator.Verify(x => x.Send(It.IsAny<GetDeleteApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(getDeleteApplicationQueryResult, options => options.ExcludingMissingMembers().Excluding(x => x.ApplicationStartDate));
        model.ApplicationStartDate.Should().Be(getDeleteApplicationQueryResult.ApplicationStartDate.ToGdsDateString());
        model.VacancyCloseDate.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, getDeleteApplicationQueryResult.VacancyClosingDate, getDeleteApplicationQueryResult.VacancyClosedDate));
        model.WorkLocation.Should().Be(VacancyDetailsHelperService.GetEmploymentLocationCityNames(getDeleteApplicationQueryResult.Addresses));
        capturedQuery.Should().NotBeNull();
        capturedQuery.ApplicationId.Should().Be(applicationId);
        capturedQuery.CandidateId.Should().Be(candidateId);
    }
}