using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;
using SFA.DAS.FAA.Domain.Apply.PostAdditionalQuestion;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Commands.AdditionalQuestion;

[TestFixture]
public class WhenHandlingAddAdditionalQuestionCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_CommandResult_Is_Returned_As_Expected(
        AddAdditionalQuestionCommand command,
        PostAdditionalQuestionApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClient,
        [Greedy] AddAdditionalQuestionCommandHandler handler)
    {
        var expectedApiRequest =
            new PostAdditionalQuestionApiRequest(command.ApplicationId, new PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData());

        apiClient.Setup(x =>
                x.Post<PostAdditionalQuestionApiResponse>(
                    It.Is<PostAdditionalQuestionApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                                                  && ((PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData)r.Data).CandidateId == command.CandidateId
                                                  && ((PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData)r.Data).Answer == command.Answer
                                                  && ((PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData)r.Data).Id == command.Id
                                                  && ((PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData)r.Data).AdditionalQuestionSectionStatus == command.AdditionalQuestionSectionStatus
                                                  && ((PostAdditionalQuestionApiRequest.PostAdditionalQuestionApiRequestData)r.Data).UpdatedAdditionalQuestion == command.UpdatedAdditionalQuestion
                    )))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(command, It.IsAny<CancellationToken>());

        result.Id.Should().Be(apiResponse.Id);
    }
}