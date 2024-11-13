using SFA.DAS.FAA.Application.Queries.User.GetSavedSearch;
using SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.SavedSearches;

public class WhenHandlingGetSavedSearchQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Saved_Search_Is_Returned(
        GetSavedSearchQuery query,
        GetSavedSearchApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetSavedSearchQueryHandler sut
        )
    {
        // arrange
        GetSavedSearchApiRequest? passedRequest = null;
        apiClient
            .Setup(x => x.Get<GetSavedSearchApiResponse>(It.IsAny<GetSavedSearchApiRequest>()))
            .Callback<IGetApiRequest>(x => passedRequest = x as GetSavedSearchApiRequest)
            .ReturnsAsync(apiResponse);
        
        // act
        var result = await sut.Handle(query, CancellationToken.None);
        
        // assert
        passedRequest.Should().NotBeNull();
        passedRequest!.GetUrl.Should().Be($"users/{query.CandidateId}/saved-searches/{query.Id}");
        result.Should().BeEquivalentTo(apiResponse);
    }
}