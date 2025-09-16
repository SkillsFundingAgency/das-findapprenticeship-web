using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_The_View_Is_Returned(
        Guid candidateId,
        [Frozen] GetSavedSearchesQueryResult savedSearchesQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
        )
    {
        // arrange
        sut.WithContext(x => x.WithUser(candidateId));
        
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedSearchesQueryResult);
        
        // act
        var actual = (await sut.GetSavedSearches(null, default) as ViewResult)?.Model as SavedSearchesViewModel;
        
        // assert
        actual.Should().NotBeNull();
        actual!.SavedSearches.Count.Should().Be(3);
        actual.SavedSearches[0].Should().BeEquivalentTo(savedSearchesQueryResult.SavedSearches[0], options => options.ExcludingMissingMembers());
        actual.ShowDeletedBanner.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task If_The_Banner_Should_Be_Shown_Then_With_The_Correct_Name(
        Guid candidateId,
        Guid savedSearchId,
        string expectedTitle,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] GetSavedSearchesQueryResult savedSearchesQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
    )
    {
        // arrange
        sut.WithContext(x => x.WithUser(candidateId));
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedSearchesQueryResult);
        string? passedCacheKey = null;
        cacheStorageService
            .Setup(x => x.Get<string?>(It.IsAny<string>()))
            .Callback<string>(x => passedCacheKey = x)
            .ReturnsAsync(expectedTitle);
        
        // act
        var actual = (await sut.GetSavedSearches(savedSearchId, default) as ViewResult)?.Model as SavedSearchesViewModel;
        
        // assert
        actual.Should().NotBeNull();
        actual!.ShowDeletedBanner.Should().BeTrue();
        actual.DeletedSavedSearchTitle.Should().Be(expectedTitle);
        passedCacheKey.Should().Be($"{candidateId}-{savedSearchId}-savedsearch");
    }
    
    [Test, MoqAutoData]
    public async Task If_The_Banner_Should_Be_Shown_Then_Without_The_Saved_Search_Name(
        Guid candidateId,
        Guid savedSearchId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] GetSavedSearchesQueryResult savedSearchesQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
    )
    {
        // arrange
        sut.WithContext(x => x.WithUser(candidateId));
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedSearchesQueryResult);
        
        cacheStorageService
            .Setup(x => x.Get<string?>(It.IsAny<string>()))
            .ReturnsAsync((string?)null);
        
        // act
        var actual = (await sut.GetSavedSearches(savedSearchId, default) as ViewResult)?.Model as SavedSearchesViewModel;
        
        // assert
        actual.Should().NotBeNull();
        actual!.ShowDeletedBanner.Should().BeTrue();
        actual.DeletedSavedSearchTitle.Should().BeNull();
    }
}