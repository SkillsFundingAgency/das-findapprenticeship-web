using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetSearchResultsApiRequest();

        actual.GetUrl.Should().Be("vacancies");
    }
}