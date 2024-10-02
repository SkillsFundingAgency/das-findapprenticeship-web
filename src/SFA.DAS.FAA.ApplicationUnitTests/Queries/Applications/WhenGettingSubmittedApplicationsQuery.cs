using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Applications.GetSubmittedApplications;
using SFA.DAS.FAA.Domain.Applications.GetSubmittedApplications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Applications
{
    public class WhenGettingSubmittedApplicationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetSubmittedApplicationsQuery query,
            GetSubmittedApplicationsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetSubmittedApplicationsQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetSubmittedApplicationsApiRequest(query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetSubmittedApplicationsApiResponse>(
                        It.Is<GetSubmittedApplicationsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.SubmittedApplications.Should().BeEquivalentTo(apiResponse.SubmittedApplications, options => options.Excluding(op => op.Status));
        }
    }
}