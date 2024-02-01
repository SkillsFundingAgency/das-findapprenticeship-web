using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Domain.Apply.AddJob;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.WorkHistory
{
    [TestFixture]
    public class WhenHandlingAddJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            AddJobCommand command,
            PostJobApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] AddJobCommandHandler handler)
        {
            var expectedApiRequest =
                new PostJobApiRequest(command.ApplicationId, new PostJobApiRequest.PostJobApiRequestData());

            apiClient.Setup(x =>
                    x.PostWithResponseCode<PostJobApiResponse>(
                        It.Is<PostJobApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).CandidateId == command.CandidateId
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).JobDescription == command.JobDescription
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).JobTitle == command.JobTitle
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).EmployerName == command.EmployerName
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).StartDate == command.StartDate
                                        && ((PostJobApiRequest.PostJobApiRequestData) r.Data).EndDate == command.EndDate
                        )))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(command, It.IsAny<CancellationToken>());

            result.Id.Should().Be(apiResponse.Id);
        }
    }
}
