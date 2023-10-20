using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
public class WhenCreatingSearchByLocationsViewModel
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(Domain.LocationsBySearch.GetLocationsBySearchApiResponse.LocationItem source)
    {
        var actual = (LocationBySearchViewModel)source;

        source.Should().BeEquivalentTo(actual);
    }
}
