using SFA.DAS.FAA.Infrastructure.Api;
using SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch;
using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SaveSearch;

public class WhenHandlingUnsubscribeSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_Command_Is_Handled_And_Api_Request_Made(
        UnsubscribeSavedSearchCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        UnsubscribeSavedSearchCommandHandler handler
        )
    {
        // Arrange
        PostSavedSearchUnsubscribeApiRequest? request = null;
        apiClient
            .Setup(client => client.PostWithResponseCode(It.IsAny<PostSavedSearchUnsubscribeApiRequest>()))
            .Callback<IPostApiRequest>(cb => request = cb as PostSavedSearchUnsubscribeApiRequest)
            .Returns(() => Task.CompletedTask);

        // Act
        var response = await handler.Handle(command, default);
        var payload = request?.Data as PostSavedSearchUnsubscribeApiRequest;

        // Assert
        response.Should

        // act
        var response = await sut.Handle(command, default);
        var payload = request?.Data as PostSaveSearchApiRequest.PostSaveSearchApiRequestData;

        // assert
        response.Should().Be(Unit.Value);
        request?.PostUrl.Should().Be($"searchapprenticeships/saved-search?candidateId={command.CandidateId}");
        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(command, options => options.Excluding(x => x.CandidateId));
    }
}