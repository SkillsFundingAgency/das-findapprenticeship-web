using SFA.DAS.FAA.Application.Commands.User.PostAccountDeletion;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.User.AccountDeletion;

[TestFixture]
public class WhenHandlingAccountDeletionCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        AccountDeletionCommand command,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] AccountDeletionCommandHandler handler)
    {
        await handler.Handle(command, It.IsAny<CancellationToken>());

        apiClient.Verify(x => x.Post(It.IsAny<IPostApiRequest>()), Times.Once);
    }
}