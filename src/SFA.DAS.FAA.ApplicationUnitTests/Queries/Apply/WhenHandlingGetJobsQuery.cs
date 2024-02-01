using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetJobsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetJobsQuery query,
            GetJobsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetJobsQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetJobsApiRequest(query.ApplicationId, query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetJobsApiResponse>(
                        It.Is<GetJobsApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(apiResponse);
        }
    }
}
