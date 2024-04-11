using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

public class WhenHandlingGetApplicationSummaryQueryHandler
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetApplicationSummaryQuery query,
        GetApplicationSummaryApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetApplicationSummaryQueryHandler handler)
    {
        // Arrange
        var apiRequestUri = new GetApplicationSummaryApiRequest(query.ApplicationId, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetApplicationSummaryApiResponse>(
                    It.Is<GetApplicationSummaryApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(apiResponse);
    }
}