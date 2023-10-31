using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchByLocationsViewModel
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(List<Domain.LocationsBySearch.GetLocationsBySearchApiResponse.LocationItem> source)
    {
        var actual = (LocationsBySearchViewModel)source;

        actual.Locations.Should().BeEquivalentTo(source,options => options.Excluding(c=>c.LocationPoint));
    }
}