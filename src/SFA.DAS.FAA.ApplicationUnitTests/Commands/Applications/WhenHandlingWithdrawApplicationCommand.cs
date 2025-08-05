using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Applications.Withdraw;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.Applications;

public class WhenHandlingWithdrawApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_Api_Request_Made(
        WithdrawApplicationCommand request,
        [Frozen] Mock<IApiClient> apiClient,
        WithdrawApplicationCommandHandler handler)
    {
        await handler.Handle(request, CancellationToken.None);

        apiClient.Verify(x => x.Post(
            It.Is<PostWithdrawApplicationApiRequest>(c =>
                c.PostUrl.Contains(request.ApplicationId.ToString()) &&
                c.PostUrl.Contains(request.CandidateId.ToString()))), Times.Once());
    }
}