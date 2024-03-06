using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.WhatInterestsYou;
using SFA.DAS.FAA.Domain.Apply.WhatInterestsYou;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.WhatInterestsYou
{
    [TestFixture]
    public class WhenHandlingUpdateWhatInterestsYouCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_As_Expected(
            UpdateWhatInterestsYouCommand command,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] UpdateWhatInterestsYouCommandHandler handler)
        {
            var expectedApiRequest =
                new PostWhatInterestsYouApiRequest(command.ApplicationId, new PostWhatInterestsYouApiRequest.PostWhatInterestsYouRequestData());

            apiClient.Setup(x =>
                    x.PostWithResponseCode(It.IsAny<PostWhatInterestsYouApiRequest>()))
                .Returns(()=> Task.CompletedTask);

            await handler.Handle(command, It.IsAny<CancellationToken>());

            apiClient.Verify(x =>
                    x.PostWithResponseCode(
                        It.Is<PostWhatInterestsYouApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                               && ((PostWhatInterestsYouApiRequest.PostWhatInterestsYouRequestData)r.Data).CandidateId == command.CandidateId
                               && ((PostWhatInterestsYouApiRequest.PostWhatInterestsYouRequestData)r.Data).AnswerText == command.AnswerText
                               && ((PostWhatInterestsYouApiRequest.PostWhatInterestsYouRequestData)r.Data).IsComplete == command.IsComplete
                        )), Times.Once);
        }
    }
}
