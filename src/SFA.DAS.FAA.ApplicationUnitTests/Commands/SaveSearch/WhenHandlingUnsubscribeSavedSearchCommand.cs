using MediatR;
using SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SaveSearch;

public class WhenHandlingUnsubscribeSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_Command_Is_Handled_And_Api_Request_Made(
        UnsubscribeSavedSearchCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        UnsubscribeSavedSearchCommandHandler sut)
    {
        // Act
        await sut.Handle(command, default);
        
        // Assert
        apiClient
            .Verify(client => client.PostWithResponseCode(
                    It.Is<PostSavedSearchUnsubscribeApiRequest>(c =>
                        c.PostUrl.Equals("saved-searches/unsubscribe") &&
                        ((PostSavedSearchUnsubscribeApiRequestData)c.Data).SavedSearchId
                        .Equals(command.SavedSearchId))), Times.Once);
    }
}