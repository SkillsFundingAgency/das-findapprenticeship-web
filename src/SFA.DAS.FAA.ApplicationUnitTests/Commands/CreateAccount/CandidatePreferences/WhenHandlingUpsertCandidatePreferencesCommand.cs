using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.CreateAccount.CandidatePreferences;
public class WhenHandlingUpsertCandidatePreferencesCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_TaskResult_Is_Returned_As_Expected(
        UpsertCandidatePreferencesCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] UpsertCandidatePreferencesCommandHandler handler)
    {
        var expectedApiRequest = new UpsertCandidatePreferencesApiRequest(command.CandidateId, new UpsertCandidatePreferencesData());

        apiClient.Setup(x =>
                x.PostWithResponseCode(It.Is<UpsertCandidatePreferencesApiRequest>(c =>
                c.PostUrl.Equals(expectedApiRequest.PostUrl))))
                .Returns(() => Task.CompletedTask);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.Should().Be(Unit.Value);
    }
}
