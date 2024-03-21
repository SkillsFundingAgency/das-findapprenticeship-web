using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateQualifications;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Qualifications;

[TestFixture]
public class WhenHandlingUpdateQualificationsCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_As_Expected(
        UpdateQualificationsCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] UpdateQualificationsCommandHandler handler)
    {
        var expectedApiRequest =
            new PostQualificationsApiRequest(command.ApplicationId, new PostQualificationsApiRequest.PostQualificationsApiRequestData());

        apiClient.Setup(x =>
                x.PostWithResponseCode(It.IsAny<PostQualificationsApiRequest>()))
            .Returns(()=> Task.CompletedTask);

        await handler.Handle(command, It.IsAny<CancellationToken>());

        apiClient.Verify(x =>
            x.PostWithResponseCode(
                It.Is<PostQualificationsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                         && ((PostQualificationsApiRequest.PostQualificationsApiRequestData)r.Data).CandidateId == command.CandidateId
                                                         && ((PostQualificationsApiRequest.PostQualificationsApiRequestData)r.Data).IsComplete == command.IsComplete
                )), Times.Once);
    }
}