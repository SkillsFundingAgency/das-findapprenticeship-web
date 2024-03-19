using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.InterviewAdjustments;
public class WhenHandlingUpdateInterviewAdjustmentsCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateInterviewAdjustmentsCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] UpdateInterviewAdjustmentsCommandHandler handler)
    {
        var expectedPostUpdateApplicationRequest = new PostInterviewAdjustmentsApiRequest(
            command.ApplicationId,
            new PostInterviewAdjustmentsModel
            {
                CandidateId = command.CandidateId,
                InterviewAdjustmentsDescription = command.InterviewAdjustmentsDescription,
                InterviewAdjustmentsSectionStatus = command.InterviewAdjustmentsSectionStatus
            });

        apiClient.Setup(client => client.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(expectedPostUpdateApplicationRequest)).ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.Application.Should().BeEquivalentTo(apiResponse);
    }
}
