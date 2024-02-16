using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.Testing.AutoFixture;


namespace SFA.DAS.FAA.Application.UnitTests.Commands.WorkHistory
{
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

            apiClient.Setup(client => client.PostWithResponseCode(It.Is<PostDeleteJobApiRequest>(r => r.PostUrl == expectedRequest.PostUrl)));

            await handler.Handle(command, CancellationToken.None);

            apiClient.Verify(x => x.PostWithResponseCode(It.IsAny<PostDeleteJobApiRequest>()), Times.Once);
        }
    }
}