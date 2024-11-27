using SFA.DAS.FAA.Application.Commands.SavedSearches.PostDeleteSavedSearch;
using SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.User;

public class WhenHandlingDeleteSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Searches_Are_Returned(
        DeleteSavedSearchCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        DeleteSavedSearchCommandHandler sut
    )
    {
        // arrange
        PostDeleteSavedSearchRequest? passedRequest = null;
        apiClient
            .Setup(x => x.PostWithResponseCode(It.IsAny<PostDeleteSavedSearchRequest>()))
            .Callback<IPostApiRequest>(x => passedRequest = x as PostDeleteSavedSearchRequest);
        
        // act
        await sut.Handle(command, CancellationToken.None);
        
        // assert
        passedRequest.Should().NotBeNull();
        passedRequest!.PostUrl.Should().Be($"users/{command.CandidateId}/saved-searches/{command.Id}/delete");
    }
}