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
            DeleteJobCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            DeleteJobCommandHandler handler)
        {
            var expectedRequest = new DeleteJobApiRequest(command.ApplicationId, command.CandidateId, command.JobId);

            apiClient.Setup(client => client.Delete(It.Is<DeleteJobApiRequest>(r => r.DeleteUrl == expectedRequest.DeleteUrl)));

            await handler.Handle(command, CancellationToken.None);

            apiClient.Verify(x => x.Delete(It.IsAny<DeleteJobApiRequest>()), Times.Once);
        }
    }
}