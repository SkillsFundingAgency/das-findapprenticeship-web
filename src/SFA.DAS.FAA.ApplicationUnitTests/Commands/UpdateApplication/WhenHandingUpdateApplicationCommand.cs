using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UpdateApplication;

[TestFixture]
public class WhenHandingUpdateApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateApplicationCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateApplicationCommandHandler handler)
    {
        var expectedPostRequest = new UpdateApplicationApiRequest(command.VacancyReference, command.ApplicationId, command.CandidateId, new UpdateApplicationModel
        {
            WorkHistorySectionStatus = command.WorkHistorySectionStatus
        });

        apiClientMock
            .Setup(client => client.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(expectedPostRequest))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Application.Should().NotBeNull();
        result.Application.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_ApiResponse_IsEmpty_The_CommandResult_Is_Returned_As_Expected(
        UpdateApplicationCommand command,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateApplicationCommandHandler handler)
    {
        var expectedPostRequest = new UpdateApplicationApiRequest(command.VacancyReference, command.ApplicationId, command.CandidateId, new UpdateApplicationModel
        {
            WorkHistorySectionStatus = command.WorkHistorySectionStatus
        });

        apiClientMock
            .Setup(client => client.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(expectedPostRequest))
            .ReturnsAsync((Domain.Apply.UpdateApplication.Application)null!);

        var result = await handler.Handle(command, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Application.Should().BeNull();
    }
}