using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.LocationsBySearch;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.LocationsBySearch;
public class WhenBuildingTheRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string searchTerm)
    {
        var actual = new GetLocationsBySearchApiRequest(searchTerm);

        actual.GetUrl.Should().Be($"locations/searchbylocation?searchTerm={searchTerm}");
    }
}
