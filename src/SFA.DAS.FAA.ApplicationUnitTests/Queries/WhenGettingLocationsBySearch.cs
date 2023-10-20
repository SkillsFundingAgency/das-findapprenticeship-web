using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.LocationsBySearch;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FAA.Domain.LocationsBySearch.GetLocationsBySearchApiResponse;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;
public class WhenGettingLocationsBySearch
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Results_From_Service(List<LocationItem> locationItems)
    {
        var apiClientMock = new Mock<IApiClient>();
        var query = new GetLocationsBySearchQuery();
        var handler = new GetLocationsBySearchQueryHandler(apiClientMock.Object);

        var expectedResponse = new GetLocationsBySearchApiResponse() { LocationItems = locationItems};
        apiClientMock.Setup(client => client.Get<GetLocationsBySearchApiResponse>(It.IsAny<GetLocationsBySearchApiRequest>()))
            .ReturnsAsync(expectedResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.LocationItems.Should().BeEquivalentTo(expectedResponse.LocationItems);
        }
    }
}
