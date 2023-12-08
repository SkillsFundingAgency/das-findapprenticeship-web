using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchResultsViewModel
{

    [Test]
    [InlineAutoData(0, "No apprenticeships found")]
    [InlineAutoData(1, "1 apprenticeship found")]
    [InlineAutoData(2, "2 apprenticeships found")]
    [InlineAutoData(2034, "2,034 apprenticeships found")]
    public void Then_The_Text_Is_Shown_Correctly_For_Number_Of_Vacancies(int numberOfVacancies, string expectedText, GetSearchResultsResult source)
    {
        source.Total = numberOfVacancies;

        var actual = (SearchResultsViewModel)source;

        Assert.That(actual.TotalMessage, Is.EqualTo(expectedText));
    }

    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetSearchResultsResult source)
    {
        var actual = (SearchResultsViewModel)source;

        actual.Location.Should().BeEquivalentTo(source.Location!.LocationName);
        actual.Routes.Should().BeEquivalentTo(source.Routes);
        
    }
}