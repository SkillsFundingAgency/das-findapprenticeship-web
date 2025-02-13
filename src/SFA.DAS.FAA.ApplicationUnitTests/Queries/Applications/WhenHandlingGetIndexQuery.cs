using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetIndexQuery query,
            GetApplicationsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetIndexQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetApplicationsApiRequest(query.CandidateId, ApplicationStatus.Draft);

            apiClientMock.Setup(client =>
                    client.Get<GetApplicationsApiResponse>(
                        It.Is<GetApplicationsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Applications.Should().BeEquivalentTo(apiResponse.Applications, options => options
                .Excluding(x => x.Address)
                .Excluding(x => x.OtherAddresses)
            );
        }
    }
}
