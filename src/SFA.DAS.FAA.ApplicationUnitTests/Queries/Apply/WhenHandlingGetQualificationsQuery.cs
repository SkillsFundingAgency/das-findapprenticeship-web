using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualifications;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries.Apply
{
    public class WhenHandlingGetQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Returned(
            GetQualificationsQuery query,
            GetQualificationsApiResponse apiResponse,
            [Frozen] Mock<IApiClient> apiClientMock,
            GetQualificationsQueryHandler handler)
        {
            // Arrange
            var apiRequestUri = new GetQualificationsApiRequest(query.ApplicationId, query.CandidateId);

            apiClientMock.Setup(client =>
                    client.Get<GetQualificationsApiResponse>(
                        It.Is<GetQualificationsApiRequest>(c =>
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
