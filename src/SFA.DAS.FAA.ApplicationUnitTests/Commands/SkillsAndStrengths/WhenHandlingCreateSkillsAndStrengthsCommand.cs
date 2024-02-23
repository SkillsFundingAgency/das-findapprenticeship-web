using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
using SFA.DAS.FAA.Domain.Apply.CreateSkillsAndStrengths;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SkillsAndStrengths;
public class WhenHandlingCreateSkillsAndStrengthsCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        CreateSkillsAndStrengthsCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] CreateSkillsAndStrengthsCommandHandler handler)
    {
        var expectedApiRequest =
            new PostSkillsAndStrengthsApiRequest(command.ApplicationId, new PostSkillsAndStrengthsApiRequest.PostCreateSkillsAndStrengthsRequestData());

        apiClient.Setup(x =>
                x.PostWithResponseCode(
                    It.Is<PostSkillsAndStrengthsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl)
                    ))
            .Returns(() => Task.CompletedTask);

        await handler.Handle(command, It.IsAny<CancellationToken>());

        apiClient.Verify(x => x.PostWithResponseCode(It.IsAny<IPostApiRequest>()), Times.Once);
    }
}
