using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchApprenticeshipsIndexRequest;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string? locationName)
    {
        locationName = $"{locationName}&!@*£(£<>{locationName}";
        
        var actual = new GetSearchApprenticeshipsIndexApiRequest(locationName);

        actual.GetUrl.Should().Be($"searchapprenticeships?locationSearchTerm={HttpUtility.UrlEncode(locationName)}");
    }
    [Test]
    public void Then_The_Url_Is_Correctly_Constructed_With_No_Params()
    {
        var actual = new GetSearchApprenticeshipsIndexApiRequest(null);

        actual.GetUrl.Should().Be($"searchapprenticeships?locationSearchTerm=");
    }
}