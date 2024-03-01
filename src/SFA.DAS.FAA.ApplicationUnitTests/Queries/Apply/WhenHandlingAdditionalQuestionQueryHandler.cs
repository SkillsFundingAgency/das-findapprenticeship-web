using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;
using SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenHandlingAdditionalQuestionQueryHandler
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetAdditionalQuestionQuery query,
        GetAdditionalQuestionApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetAdditionalQuestionQueryHandler handler)
    {
        // Arrange
        var apiRequestUri = new GetAdditionalQuestionApiRequest(query.ApplicationId, query.CandidateId, query.AdditionQuestionId);

        apiClientMock.Setup(client =>
                client.Get<GetAdditionalQuestionApiResponse>(
                    It.Is<GetAdditionalQuestionApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
    }
}