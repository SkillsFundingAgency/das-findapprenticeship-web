using SFA.DAS.FAA.Domain.LocationsBySearch;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchByLocationViewModel
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetLocationsBySearchApiResponse.LocationItem source)
    {
        var actual = (LocationBySearchViewModel)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.LocationPoint));
    }
}