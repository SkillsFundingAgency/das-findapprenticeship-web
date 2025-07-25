using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.Applications.Delete;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications;

public class WhenPostingDelete
{
    [Test, MoqAutoData]
    public async Task Delete_Redirects_To_Index_When_ApplicationId_Is_Empty(
        [Greedy] ApplicationsController sut)
    {
        // act
        var result = await sut.Delete(Guid.Empty, CancellationToken.None) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        result.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        result.RouteValues.Should().Contain(x => x.Key == "tab" && x.Value!.ToString() == "Started");
    }
    
    [Test, MoqAutoData]
    public async Task Stores_Message_When_Delete_Succeeds(
        Guid candidateId,
        Guid applicationId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController sut)
    {
        // arrange
        DeleteApplicationCommand? capturedQuery = null;
        sut.AddControllerContext().WithUser(candidateId);
        mediator
            .Setup(x => x.Send(It.IsAny<DeleteApplicationCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<DeleteApplicationCommandResult>, CancellationToken>((query, _) => { capturedQuery = query as DeleteApplicationCommand; })
            .ReturnsAsync(new DeleteApplicationCommandResult { Success = true, EmployerName = "employerName", VacancyTitle = "vacancyTitle"});
        const string expectedMessage = "Application deleted for vacancyTitle at employerName";
        
        // act
        var result = await sut.Delete(applicationId, CancellationToken.None) as RedirectToRouteResult;
        
        // assert
        mediator.Verify(x => x.Send(It.IsAny<DeleteApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        cacheStorageService.Verify(x => x.Set(It.IsAny<string>(), expectedMessage, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        result.Should().NotBeNull();
        result.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        result.RouteValues.Should().Contain(x => x.Key == "tab" && x.Value!.ToString() == "Started");
        capturedQuery.Should().NotBeNull();
        capturedQuery.ApplicationId.Should().Be(applicationId);
        capturedQuery.CandidateId.Should().Be(candidateId);
    }
    
    [Test, MoqAutoData]
    public async Task Does_Not_Store_Message_When_Delete_Fails(
        Guid candidateId,
        Guid applicationId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController sut)
    {
        // arrange
        DeleteApplicationCommand? capturedQuery = null;
        sut.AddControllerContext().WithUser(candidateId);
        mediator
            .Setup(x => x.Send(It.IsAny<DeleteApplicationCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<DeleteApplicationCommandResult>, CancellationToken>((query, _) => { capturedQuery = query as DeleteApplicationCommand; })
            .ReturnsAsync(new DeleteApplicationCommandResult { Success = false});
        
        // act
        var result = await sut.Delete(applicationId, CancellationToken.None) as RedirectToRouteResult;
        
        // assert
        mediator.Verify(x => x.Send(It.IsAny<DeleteApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        cacheStorageService.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        result.Should().NotBeNull();
        result.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        result.RouteValues.Should().Contain(x => x.Key == "tab" && x.Value!.ToString() == "Started");
        capturedQuery.Should().NotBeNull();
        capturedQuery.ApplicationId.Should().Be(applicationId);
        capturedQuery.CandidateId.Should().Be(candidateId);
    }
}