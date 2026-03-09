using SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SkillsAndStrengths;

public class WhenHandlingUpdateSkillsAndStrengthsCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        UpdateSkillsAndStrengthsCommand command,
        Domain.Apply.UpdateApplication.Application apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] UpdateSkillsAndStrengthsCommandHandler handler)
    {
        var expectedPostUpdateApplicationRequest = new PostSkillsAndStrengthsApiRequest(command.ApplicationId, new PostSkillsAndStrengthsModel
        {
            CandidateId = command.CandidateId,
            SkillsAndStrengths = command.SkillsAndStrengths,
            SkillsAndStrengthsSectionStatus = command.SkillsAndStrengthsSectionStatus
        });

        apiClient.Setup(client => client.Post<Domain.Apply.UpdateApplication.Application>(expectedPostUpdateApplicationRequest)).ReturnsAsync(apiResponse);

        var expectedPostSkillsAndStrengthsApiRequest =
            new Domain.Apply.CreateSkillsAndStrengths.PostSkillsAndStrengthsApiRequest(command.ApplicationId, new Domain.Apply.CreateSkillsAndStrengths.PostSkillsAndStrengthsApiRequest.PostCreateSkillsAndStrengthsRequestData());

        apiClient.Setup(x =>
                x.Post(
                    It.Is<Domain.Apply.CreateSkillsAndStrengths.PostSkillsAndStrengthsApiRequest>(r => r.PostUrl == expectedPostSkillsAndStrengthsApiRequest.PostUrl)
                    ))
            .Returns(() => Task.CompletedTask);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.Application.Should().BeEquivalentTo(apiResponse);
    }
}
