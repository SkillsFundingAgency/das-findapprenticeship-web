using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.SubmitApplication;
using SFA.DAS.FAA.Domain.Apply.SubmitPreviewApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SubmitApplication;

public class WhenHandlingSubmitApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Request_Submitted(
        SubmitApplicationCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        SubmitApplicationCommandHandler handler)
    {
        var expectedRequest = new SubmitPreviewApplicationRequest(command.CandidateId, command.ApplicationId);
        
        await handler.Handle(command, CancellationToken.None);

        apiClient.Verify(
            x => x.PostWithResponseCode(It.Is<SubmitPreviewApplicationRequest>(c => c.PostUrl == expectedRequest.PostUrl)),
            Times.Once);
    }
}