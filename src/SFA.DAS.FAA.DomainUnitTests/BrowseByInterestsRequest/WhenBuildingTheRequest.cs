using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.BrowseByInterests;

namespace SFA.DAS.FAA.Domain.UnitTests.BrowseByInterestsRequest;

public class WhenBuildingTheRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetBrowseByInterestsApiRequest();

        actual.GetUrl.Should().Be("searchapprenticeships/browsebyinterests");
    }
}