using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.SaveSearch;

public class WhenHandlingUnsubscribeSavedSearchCommand
{
    [Test, MoqAutoData]
    public async Task Then_Command_Is_Handled_And_Api_Request_Made(
        UnsubscribeSavedSearchCommand command,
        [Frozen] ApiClient apiClient,
        UnsubscribeSavedSearchCommandHandler handler
        )
    {
        
    }
}