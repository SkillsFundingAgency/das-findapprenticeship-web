using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSearchResultsViewModel
{

    [Test]
    [InlineAutoData(0, 0, "No vacancies found")]
    [InlineAutoData(1, 0, "1 vacancy found")]
    [InlineAutoData(2, 0, "2 vacancies found")]
    [InlineAutoData(2034, 20, "2,054 vacancies found")]
    public void Then_The_Text_Is_Shown_Correctly_For_Number_Of_Vacancies(int numberOfVacancies, int numberOfCompetitiveVacancies, string expectedText, GetSearchResultsResult source)
    {
        source.Total = numberOfVacancies;
        source.TotalCompetitiveVacanciesCount = numberOfCompetitiveVacancies;

        var actual = (SearchResultsViewModel)source;

        Assert.That(actual.TotalMessage, Is.EqualTo(expectedText));
    }

    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetSearchResultsResult source)
    {
        var actual = (SearchResultsViewModel)source;

        actual.Location.Should().BeEquivalentTo(source.Location!.LocationName);
        actual.Routes.Should().BeEquivalentTo(source.Routes);
        actual.Levels.Should().BeEquivalentTo(source.Levels, opts => opts.Excluding(c =>c.Code));

    }
}