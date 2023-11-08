using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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
        GetLocationsBySearchApiResponse expectedResponse,
        [Frozen] Mock<IApiClient> apiClientMock,
        GetLocationsBySearchQueryHandler handler)
    {
        apiClientMock.Setup(client => client.Get<GetLocationsBySearchApiResponse>(
                It.Is<GetLocationsBySearchApiRequest>(c =>
                    c.GetUrl.Contains($"locations/searchbylocation?searchTerm={query.SearchTerm}"))))
            .ReturnsAsync(expectedResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.LocationItems.Should().BeEquivalentTo(expectedResponse.LocationItems);
        }
    }
}
