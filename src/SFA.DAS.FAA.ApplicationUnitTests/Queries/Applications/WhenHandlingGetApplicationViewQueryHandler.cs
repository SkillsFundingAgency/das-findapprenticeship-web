using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    public class WhenHandlingGetApplicationViewQueryHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetApplicationViewQuery query,
            GetApplicationViewApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetApplicationViewQueryHandler handler)
        {
            // Arrange
            apiResponse.ApplicationStatus = "Withdrawn";
            var apiRequestUri = new GetApplicationViewApiRequest(query.ApplicationId, query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetApplicationViewApiResponse>(
                        It.Is<GetApplicationViewApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(apiResponse, options => options.Excluding(c => c.ApplicationStatus));
            result.ApplicationStatus.Should().Be(ApplicationStatus.Withdrawn);
            result.WithdrawnDate.Should().Be(apiResponse.WithdrawnDate);
            result.MigrationDate.Should().Be(apiResponse.MigrationDate);
            result.ApprenticeshipType.Should().Be(apiResponse.ApprenticeshipType);
        }
    }
}