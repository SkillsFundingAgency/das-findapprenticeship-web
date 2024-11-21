using SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.SavedSearches;

public class WhenHandlingGetSavedSearchesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Searches_Are_Returned(
        GetSavedSearchesQuery query,
        GetSavedSearchesApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetSavedSearchesQueryHandler sut
        )
    {
        // arrange
        GetSavedSearchesApiRequest? passedRequest = null;
        apiClient
            .Setup(x => x.Get<GetSavedSearchesApiResponse>(It.IsAny<GetSavedSearchesApiRequest>()))
            .Callback<IGetApiRequest>(x => passedRequest = x as GetSavedSearchesApiRequest)
            .ReturnsAsync(apiResponse);
        
        // act
        var result = await sut.Handle(query, CancellationToken.None);
        
        // assert
        passedRequest.GetUrl.Should().Be($"users/{query.CandidateId}/saved-searches");
        result.Should().BeEquivalentTo(apiResponse);
    }
}