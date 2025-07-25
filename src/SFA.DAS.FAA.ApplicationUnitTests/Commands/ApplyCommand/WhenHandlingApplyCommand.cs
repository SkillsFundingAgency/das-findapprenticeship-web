using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Apply;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Vacancies.VacancyDetails;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.ApplyCommand;

[TestFixture]
public class WhenHandlingApplyCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        Application.Commands.Apply.ApplyCommand command,
        PostApplicationDetailsApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        ApplyCommandHandler handler)
    {
        var expectedApiRequest = new PostApplicationDetailsApiRequest(command.VacancyReference, new PostApplicationDetailsApiRequest.RequestBody(command.CandidateId));

        apiClientMock
            .Setup(x => x.Post<PostApplicationDetailsApiResponse>(It.Is<PostApplicationDetailsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        using var scope = new AssertionScope();
        result.ApplicationId.Should().Be(apiResponse.ApplicationId);
    }
}