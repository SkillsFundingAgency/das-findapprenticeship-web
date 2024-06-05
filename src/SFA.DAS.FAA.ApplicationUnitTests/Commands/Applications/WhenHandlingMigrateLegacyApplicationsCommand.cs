using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Applications.LegacyApplications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Applications
{
    public class WhenHandlingMigrateLegacyApplicationsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Handled_And_Api_Request_Made(
            MigrateLegacyApplicationsCommand request,
            [Frozen] Mock<IApiClient> apiClient,
            MigrateLegacyApplicationsCommandHandler handler)
        {
            await handler.Handle(request, CancellationToken.None);

            apiClient.Verify(x => x.PostWithResponseCode(
                It.Is<PostMigrateDataTransferApiRequest>(c =>
                    c.PostUrl.Contains(request.CandidateId.ToString()))), Times.Once());
        }
    }
}
