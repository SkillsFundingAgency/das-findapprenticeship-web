using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetTransferUserData;
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
            GetMigrateDataTransferApiResponse legacyApplicationsApiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetTransferUserDataQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetMigrateDataTransferApiRequest(query.EmailAddress, query.CandidateId);
            apiClientMock.Setup(client =>
                    client.Get<GetMigrateDataTransferApiResponse>(
                        It.Is<GetMigrateDataTransferApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(legacyApplicationsApiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.CandidateFirstName.Should().Be(legacyApplicationsApiResponse.FirstName);
            result.CandidateLastName.Should().Be(legacyApplicationsApiResponse.LastName);

            result.SubmittedApplications.Should().Be(legacyApplicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Submitted));
            result.SavedApplications.Should().Be(legacyApplicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Saved));
            result.StartedApplications.Should().Be(legacyApplicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Draft));
        }
    }
}