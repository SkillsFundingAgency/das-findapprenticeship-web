using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.DisabilityConfident;

public class WhenHandlingUpdateDisabilityConfidenceApplication
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateDisabilityConfidenceApplicationCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] UpdateDisabilityConfidenceApplicationCommandHandler handler)
    {
        var expectedPostUpdateApplicationRequest = new UpdateDisabilityConfidenceApplicationApiRequest(
            command.ApplicationId,
            command.CandidateId,
            new UpdateDisabilityConfidenceApplicationModel
            {
                IsSectionCompleted = command.IsSectionCompleted
            });

        apiClient.Setup(client => client.Post<Domain.Apply.UpdateApplication.Application>(expectedPostUpdateApplicationRequest)).ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.Application.Should().BeEquivalentTo(apiResponse);
    }
}