using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.User.GetSavedSearch;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenConfirmingDeleteSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_View_Is_Returned(
        Guid candidateId,
        Guid savedSearchId,
        [Frozen] GetSavedSearchQueryResult savedSearchQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
    )
    {
        // arrange
        sut.WithContext(x => x.WithUser(candidateId));

        GetSavedSearchQuery? passedQuery = null;
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<GetSavedSearchQueryResult>, CancellationToken>((x, _) => passedQuery = x as GetSavedSearchQuery)
            .ReturnsAsync(savedSearchQueryResult);
        
        // act
        var actual = (await sut.ConfirmDeleteSavedSearch(savedSearchId, default) as ViewResult)?.Model as SavedSearchViewModel;
        
        // assert
        actual.Should().NotBeNull();
        passedQuery.Should().NotBeNull();
        passedQuery!.Id.Should().Be(savedSearchId);
        passedQuery.CandidateId.Should().Be(candidateId);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Not_Found_Is_Returned(
        Guid candidateId,
        Guid savedSearchId,
        [Frozen] GetSavedSearchQueryResult savedSearchQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
    )
    {
        // arrange
        sut.WithContext(x => x.WithUser(candidateId));

        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetSavedSearchQueryResult(null, []));
        
        // act
        var actual = await sut.ConfirmDeleteSavedSearch(savedSearchId, default) as NotFoundResult;
        
        // assert
        actual.Should().NotBeNull();
    }
}