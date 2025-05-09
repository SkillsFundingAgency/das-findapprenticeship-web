using SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetApplicationCountQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetApplicationsCountQuery query,
            GetApplicationsCountApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            [Greedy] GetApplicationsCountQueryHandler handler)
        {
            // Arrange
            apiResponse.Status = nameof(ApplicationStatus.Successful);
            var apiRequestUri = new GetApplicationsCountApiRequest(query.CandidateId, query.Status);
            apiClientMock.Setup(client =>
                    client.Get<GetApplicationsCountApiResponse>(
                        It.Is<GetApplicationsCountApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            result.Stats.Should().BeEquivalentTo(apiResponse, c => c
                .Excluding(x => x.Status));
        }
    }
}