using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.DisabilityConfident;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.DisabilityConfident
{
    [TestFixture]
    public class WhenHandlingUpdateDisabilityConfidentCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_As_Expected(
            UpdateDisabilityConfidentCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] UpdateDisabilityConfidentCommandHandler handler)
        {
            var expectedApiRequest =
                new PostDisabilityConfidentApiRequest(command.ApplicationId, new PostDisabilityConfidentApiRequest.PostDisabilityConfidentApiRequestData());

            apiClient.Setup(x =>
                    x.PostWithResponseCode(It.IsAny<PostDisabilityConfidentApiRequest>()))
                .Returns(()=> Task.CompletedTask);

            await handler.Handle(command, It.IsAny<CancellationToken>());

            apiClient.Verify(x =>
                    x.PostWithResponseCode(
                        It.Is<PostDisabilityConfidentApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                               && ((PostDisabilityConfidentApiRequest.PostDisabilityConfidentApiRequestData)r.Data).CandidateId == command.CandidateId
                               && ((PostDisabilityConfidentApiRequest.PostDisabilityConfidentApiRequestData)r.Data).ApplyUnderDisabilityConfidentScheme == command.ApplyUnderDisabilityConfidentScheme
                        )), Times.Once);
        }
    }
}
