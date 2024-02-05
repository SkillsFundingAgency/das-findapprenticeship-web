using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.WorkHistory.UpdateJob;
using SFA.DAS.FAA.Domain.Apply.AddJob;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.WorkHistory
{
    [TestFixture]
    public class WhenHandlingUpdateJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            UpdateJobCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] UpdateJobCommandHandler handler)
        {
            var expectedApiRequest =
                new PostUpdateJobApiRequest(command.ApplicationId, command.JobId, new PostUpdateJobApiRequest.PostUpdateJobApiRequestData());

            apiClient.Setup(x =>
                    x.PostWithResponseCode(
                        It.Is<PostJobApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).CandidateId == command.CandidateId
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).JobDescription == command.JobDescription
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).JobTitle == command.JobTitle
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).EmployerName == command.EmployerName
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).StartDate == command.StartDate
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).EndDate == command.EndDate
                        )))
                .Returns(() => Task.CompletedTask);

            await handler.Handle(command, It.IsAny<CancellationToken>());

            apiClient.Verify(x => x.PostWithResponseCode(It.IsAny<IPostApiRequest>()), Times.Once);
        }
    }
}
