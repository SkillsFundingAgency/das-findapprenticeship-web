using SFA.DAS.FAA.Domain.LocationsBySearch;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchByLocationsViewModel
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(List<GetLocationsBySearchApiResponse.LocationItem> source)
    {
        var actual = (LocationsBySearchViewModel)source;

        actual.Locations.Should().BeEquivalentTo(source,options => options.Excluding(c=>c.LocationPoint));
    }
}