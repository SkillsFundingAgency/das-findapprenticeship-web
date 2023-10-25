using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.GeoPoint;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.GeopointRequest;
public class WhenBuildingTheRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string postCode)
    {
        var actual = new GetGeoPointApiRequest(postCode);

        actual.GetUrl.Should().Be($"locations/geopoint?postcode={postCode}");
    }
}
