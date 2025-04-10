using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocation;
using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocation;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply;

[TestFixture]
public class WhenHandlingGetEmploymentLocationsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Returned(
        GetEmploymentLocationQuery query,
        GetEmploymentLocationApiResponse apiResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetEmploymentLocationQueryHandler handler)
    {
        // Arrange
        var apiRequestUri = new GetEmploymentLocationApiRequest(query.ApplicationId, query.CandidateId);

        apiClientMock.Setup(client =>
                client.Get<GetEmploymentLocationApiResponse>(
                    It.Is<GetEmploymentLocationApiRequest>(c =>
                        c.GetUrl == apiRequestUri.GetUrl)))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(apiResponse);
    }
}