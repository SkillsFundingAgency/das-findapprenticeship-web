using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Applications.GetTransferUserData;
using SFA.DAS.FAA.Domain.Applications.GetLegacyApplications;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    public class WhenGettingTransferUserDataQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetTransferUserDataQuery query,
            GetLegacyApplicationsApiResponse legacyApplicationsApiResponse,
            GetCandidateNameApiResponse candidateNameApiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetTransferUserDataQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetLegacyApplicationsApiRequest(query.EmailAddress);
            apiClientMock.Setup(client =>
                    client.Get<GetLegacyApplicationsApiResponse>(
                        It.Is<GetLegacyApplicationsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(legacyApplicationsApiResponse);

            var candidateApiRequestUri = new GetCandidateNameApiRequest(query.CandidateId);
            apiClientMock.Setup(client =>
                    client.Get<GetCandidateNameApiResponse>(
                        It.Is<GetCandidateNameApiRequest>(c =>
                            c.GetUrl == candidateApiRequestUri.GetUrl)))
                .ReturnsAsync(candidateNameApiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.CandidateFirstName.Should().Be(candidateNameApiResponse.FirstName);
            result.CandidateLastName.Should().Be(candidateNameApiResponse.LastName);
            result.CandidateEmailAddress.Should().Be(query.EmailAddress);

            result.SubmittedApplications.Should().Be(legacyApplicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Submitted));
            result.SavedApplications.Should().Be(legacyApplicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Saved));
            result.StartedApplications.Should().Be(legacyApplicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Draft));
        }
    }
}