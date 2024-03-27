using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetDisabilityConfidentDetailsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetDisabilityConfidentDetailsQuery query,
            GetDisabilityConfidentDetailsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetDisabilityConfidentDetailsQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetDisabilityConfidentDetailsApiRequest(query.ApplicationId, query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetDisabilityConfidentDetailsApiResponse>(
                        It.Is<GetDisabilityConfidentDetailsApiRequest>(c =>
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
