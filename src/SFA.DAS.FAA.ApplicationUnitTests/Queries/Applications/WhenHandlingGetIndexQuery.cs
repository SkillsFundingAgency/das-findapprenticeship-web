using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    public class WhenHandlingGetIndexQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetIndexQuery query,
            GetApplicationsApiResponse apiResponse,
            GetInformApiResponse candidateApiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetIndexQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetApplicationsApiRequest(query.CandidateId, ApplicationStatus.Draft);
            var candidateApiRequest = new GetInformApiRequest(query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetApplicationsApiResponse>(
                        It.Is<GetApplicationsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            apiClientMock.Setup(client =>
                    client.Get<GetInformApiResponse>(
                        It.Is<GetInformApiRequest>(c =>
                            c.GetUrl == candidateApiRequest.GetUrl)))
                .ReturnsAsync(candidateApiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Applications.Should().BeEquivalentTo(apiResponse.Applications);
            result.ShowAccountRecoveryBanner.Should().Be(candidateApiResponse.ShowAccountRecoveryBanner);
        }
    }
}
