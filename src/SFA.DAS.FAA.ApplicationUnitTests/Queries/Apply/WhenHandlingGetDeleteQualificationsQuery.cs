using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetDeleteQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetDeleteQualificationsQuery query,
            GetDeleteQualificationsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetDeleteQualificationsQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetDeleteQualificationsApiRequest(query.ApplicationId, query.CandidateId, query.QualificationType, query.Id);

            apiClientMock.Setup(client =>
                    client.Get<GetDeleteQualificationsApiResponse>(
                        It.Is<GetDeleteQualificationsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_Response_Is_NotFound_Null_Is_Returned(
            GetDeleteQualificationsQuery query,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetDeleteQualificationsQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetDeleteQualificationsApiRequest(query.ApplicationId, query.CandidateId, query.QualificationType, query.Id);

            apiClientMock.Setup(client =>
                    client.Get<GetDeleteQualificationsApiResponse>(
                        It.Is<GetDeleteQualificationsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync((GetDeleteQualificationsApiResponse)null!);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
