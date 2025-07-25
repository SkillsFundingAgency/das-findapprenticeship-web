using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.WorkHistory;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UpdateApplication;

[TestFixture]
public class WhenHandingUpdateWorkHistoryApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateWorkHistoryApplicationCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateWorkHistoryApplicationCommandHandler handler)
    {
        var expectedPostRequest = new UpdateWorkHistoryApplicationApiRequest(command.ApplicationId, command.CandidateId,
            new UpdateWorkHistoryApplicationModel
            {
                WorkHistorySectionStatus = command.WorkHistorySectionStatus
            });

        apiClientMock
            .Setup(client => client.Post<Domain.Apply.UpdateApplication.Application>(expectedPostRequest))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Application.Should().NotBeNull();
        result.Application.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_ApiResponse_IsEmpty_The_CommandResult_Is_Returned_As_Expected(
        UpdateWorkHistoryApplicationCommand command,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateWorkHistoryApplicationCommandHandler handler)
    {
        var expectedPostRequest = new UpdateWorkHistoryApplicationApiRequest(command.ApplicationId, command.CandidateId,
            new UpdateWorkHistoryApplicationModel
            {
                WorkHistorySectionStatus = command.WorkHistorySectionStatus
            });

        apiClientMock
            .Setup(client => client.Post<Domain.Apply.UpdateApplication.Application>(expectedPostRequest))
            .ReturnsAsync((Domain.Apply.UpdateApplication.Application)null!);

        var result = await handler.Handle(command, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Application.Should().BeNull();
    }
}