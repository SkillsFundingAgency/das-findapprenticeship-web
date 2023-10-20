using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetLocationsBySearch;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.LocationsBySearch;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Application.UnitTests.Queries;
public class WhenGettingLocationsBySearch
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Results_From_Service(
    GetLocationsBySearchQuery query,
    GetLocationsBySearchApiResponse locationsFromService,
    [Frozen] Mock<IApiClient> mockService,
    GetLocationsBySearchQueryHandler handler)
    {
        var request = new GetLocationsBySearchApiRequest(query.SearchTerm);
        mockService.Setup(service => service.Get<GetLocationsBySearchApiResponse>(request)).ReturnsAsync(locationsFromService);

        var result = await handler.Handle(query, CancellationToken.None);

        result.LocationItems.Should().BeEquivalentTo(locationsFromService.LocationItems);
    }
}
