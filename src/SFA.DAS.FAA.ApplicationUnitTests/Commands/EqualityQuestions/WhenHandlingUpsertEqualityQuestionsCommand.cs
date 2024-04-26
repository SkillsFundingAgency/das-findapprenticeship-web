using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.EqualityQuestions;
using SFA.DAS.FAA.Domain.Apply.EqualityQuestions;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.EqualityQuestions
{
    public class WhenHandlingUpsertEqualityQuestionsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            CreateEqualityQuestionsCommand request,
            PostEqualityQuestionsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClient,
            [Greedy] CreateEqualityQuestionsCommandHandler handler)
        {
            var postUpdateApplicationRequest = new PostEqualityQuestionsApiRequest(
                request.ApplicationId,
                new UpdateEqualityQuestionsModel
                {
                    CandidateId = request.CandidateId,
                    Sex = request.Sex,
                    EthnicGroup = request.EthnicGroup,
                    EthnicSubGroup = request.EthnicSubGroup,
                    IsGenderIdentifySameSexAtBirth = request.IsGenderIdentifySameSexAtBirth,
                    OtherEthnicSubGroupAnswer = request.OtherEthnicSubGroupAnswer
                });

            apiClient.Setup(client => client.PostWithResponseCode<PostEqualityQuestionsApiResponse>(postUpdateApplicationRequest)).ReturnsAsync(apiResponse);

            var result = await handler.Handle(request, It.IsAny<CancellationToken>());

            result.Should().NotBeNull();
        }
    }
}
