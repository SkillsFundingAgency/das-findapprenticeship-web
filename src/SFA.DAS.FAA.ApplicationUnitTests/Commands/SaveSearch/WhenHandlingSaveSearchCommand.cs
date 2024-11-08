using MediatR;
using SFA.DAS.FAA.Application.Commands.SavedSearches.PostSaveSearch;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SaveSearch;

public class WhenHandlingSaveSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        SaveSearchCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] SaveSearchCommandHandler sut)
    {
        // arrange
        PostSaveSearchApiRequest? request = null;
        apiClient
            .Setup(client => client.PostWithResponseCode(It.IsAny<PostSaveSearchApiRequest>()))
            .Callback<IPostApiRequest>(cb => request = cb as PostSaveSearchApiRequest)
            .Returns(() => Task.CompletedTask);

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