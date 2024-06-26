﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.ApplicationStatus;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.UpdateApplication;

public class WhenHandlingUpdateApplicationStatusCommandHandler
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateApplicationStatusCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateApplicationStatusCommandHandler handler)
    {
        var expectedPostRequest = new UpdateApplicationStatusApiRequest(command.ApplicationId, command.CandidateId, new UpdateApplicationStatusModel
        {
            Status = command.Status
        });
        apiClientMock.Setup(client => client.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(expectedPostRequest)).ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Application.Should().NotBeNull();
            result.Application.Should().BeEquivalentTo(apiResponse);
        }
    }

    [Test, MoqAutoData]
    public async Task And_ApiResponse_Is_Empty_Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateApplicationStatusCommand command,
        [Frozen] Mock<IApiClient> apiClientMock,
        UpdateApplicationStatusCommandHandler handler)
    {
        var expectedPostRequest = new UpdateApplicationStatusApiRequest(command.ApplicationId, command.CandidateId, new UpdateApplicationStatusModel
        {
            Status = command.Status
        });
        apiClientMock.Setup(client => client.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(expectedPostRequest)).ReturnsAsync((Domain.Apply.UpdateApplication.Application)null!);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
    }
}