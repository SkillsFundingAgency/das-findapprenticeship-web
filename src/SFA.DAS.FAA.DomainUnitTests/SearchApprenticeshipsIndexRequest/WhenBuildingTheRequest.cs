using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchApprenticeshipsIndexRequest;

public class WhenBuildingTheRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetSearchApprenticeshipsIndexApiRequest();

        actual.GetUrl.Should().Be("searchapprenticeships");
    }
}