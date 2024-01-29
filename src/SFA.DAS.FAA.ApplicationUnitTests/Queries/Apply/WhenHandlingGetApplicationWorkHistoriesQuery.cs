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
    public class WhenHandlingGetApplicationWorkHistoriesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetApplicationWorkHistoriesQuery query,
            List<WorkHistory> apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetApplicationWorkHistoriesQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetApplicationWorkHistoriesApiRequest(query.CandidateId, query.ApplicationId);

            apiClientMock.Setup(client =>
                    client.Get<List<WorkHistory>>(
                        It.Is<GetApplicationWorkHistoriesApiRequest>(c =>
                            c.GetUrl == apiRequestUri.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.WorkHistories.Should().BeEquivalentTo(apiResponse);
        }
    }
}
