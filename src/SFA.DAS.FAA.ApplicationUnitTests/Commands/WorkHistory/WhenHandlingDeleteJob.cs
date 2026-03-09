using SFA.DAS.FAA.Application.Commands.WorkHistory.DeleteJob;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.WorkHistory;

public class WhenHandlingDeleteJob
{
    [Test, MoqAutoData]
    public async Task The_Job_Is_Deleted(
        PostDeleteJobCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        PostDeleteJobCommandHandler handler)
    {
        var expectedRequest = new PostDeleteJobApiRequest(command.ApplicationId, command.JobId, new PostDeleteJobApiRequest.PostDeleteJobApiRequestData
        {
            CandidateId = command.CandidateId,
        });

        apiClient.Setup(client => client.Post(It.Is<PostDeleteJobApiRequest>(r => r.PostUrl == expectedRequest.PostUrl)));

        await handler.Handle(command, CancellationToken.None);

        apiClient.Verify(x => x.Post(It.IsAny<PostDeleteJobApiRequest>()), Times.Once);
    }
}