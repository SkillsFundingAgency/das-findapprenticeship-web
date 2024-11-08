using SFA.DAS.FAA.Application.Queries.SavedSearches;
using SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.SavedSearches;

public class WhenHandlingGetConfirmUnsubscribeQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned_From_The_Api(
        GetConfirmUnsubscribeQuery query,
        ConfirmSavedSearchUnsubscribeApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetConfirmUnsubscribeQueryHandler handler)
    {
        var request = new GetConfirmSavedSearchUnsubscribeApiRequest(query.SavedSearchId);
        apiClient.Setup(x =>
                x.Get<ConfirmSavedSearchUnsubscribeApiResponse>(
                    It.Is<GetConfirmSavedSearchUnsubscribeApiRequest>(c => c.GetUrl.Equals(request.GetUrl))))
            .ReturnsAsync(apiResponse);
        
        var actual = await handler.Handle(query, CancellationToken.None);
        
        actual.SavedSearch.Should().BeEquivalentTo(apiResponse.SavedSearch);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_No_Record_Found_Null_Returned(        
        GetConfirmUnsubscribeQuery query,
        ConfirmSavedSearchUnsubscribeApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        GetConfirmUnsubscribeQueryHandler handler)
    {
        var request = new GetConfirmSavedSearchUnsubscribeApiRequest(query.SavedSearchId);
        apiClient.Setup(x =>
                x.Get<ConfirmSavedSearchUnsubscribeApiResponse>(
                    It.Is<GetConfirmSavedSearchUnsubscribeApiRequest>(c => c.GetUrl.Equals(request.GetUrl))))
            .ReturnsAsync(new ConfirmSavedSearchUnsubscribeApiResponse{SavedSearch = null});

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.SavedSearch.Should().BeNull();
    }
}