using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Domain.Apply.GetDisabilityConfident;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetDisabilityConfidentQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetDisabilityConfidentQuery query,
            GetDisabilityConfidentApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetDisabilityConfidentQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetDisabilityConfidentApiRequest(query.ApplicationId, query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetDisabilityConfidentApiResponse>(
                        It.Is<GetDisabilityConfidentApiRequest>(c =>
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
