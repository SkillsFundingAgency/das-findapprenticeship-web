using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetGeoPoint;
using SFA.DAS.FAA.Domain.GeoPoint;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;
public class WhenGettingGeoPoint
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Results_From_Service(GeoPoint geoPoint)
    {
        var apiClientMock = new Mock<IApiClient>();
        var query = new GetGeoPointQuery();
        var handler = new GetGeoPointQueryHandler(apiClientMock.Object);

        var expectedResponse = new GetGeoPointApiResponse() {  GeoPoint = geoPoint };
        apiClientMock.Setup(client => client.Get<GetGeoPointApiResponse>(It.IsAny<GetGeoPointApiRequest>()))
            .ReturnsAsync(expectedResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<GetGeoPointQueryResult>();
            result.PostCode.Should().BeEquivalentTo(expectedResponse.GeoPoint.Postcode);
            result.Latitude.Should().Be(expectedResponse.GeoPoint.Latitude);
            result.Longitude.Should().Be(expectedResponse.GeoPoint.Longitude);
        }
    }
}
