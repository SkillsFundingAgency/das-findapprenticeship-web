using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetJob;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetJobQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetJobQuery query,
            GetJobApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetJobQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetJobApiRequest(query.ApplicationId, query.CandidateId, query.JobId);

            apiClientMock.Setup(client =>
                    client.Get<GetJobApiResponse>(
                        It.Is<GetJobApiRequest>(c =>
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
