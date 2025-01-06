using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Models.User;

public class WhenCreatingSavedSearchViewModel
{
    private static readonly List<RouteInfo> RouteInfos =
    [
        new(1, "Route One"),
        new(2, "Route Two"),
        new(3, "Route Three"),
        new(4, "Route Four"),
        new(5, "Route Five")
    ];
    
    private static readonly object[] TitleTestCases =
    [
        new object?[] { "Foo", new [] {1, 2}, new [] {1, 2}, null, true, "Foo in all of England" },
        new object?[] { "Foo", new [] {1, 2}, new [] {1, 2}, "Hull", true, "Foo in Hull" },
        new object?[] { "Foo", new [] {1, 2}, new [] {1, 2}, null, true, "Foo in all of England" },
        new object?[] { null, new [] {1, 2, 3}, new [] {1, 2}, null, true, "3 categories in all of England" },
        new object?[] { null, new [] { 2 }, new [] {1, 2}, null, true, "Route Two in all of England" },
        new object?[] { null, null, new [] {1, 2, 3, 4}, null, true, "4 apprenticeship levels in all of England" },
        new object?[] { null, null, new [] { 4 }, null, true, "Level 4 in all of England" },
        new object?[] { null, null, null, null, true, "Disability Confident in all of England" },
        new object?[] { null, null, null, null, false, "All apprenticeships in all of England" },
        new object?[] { null, null, null, "Hull", false, "All apprenticeships in Hull" },
    ];
    
    [TestCaseSource(nameof(TitleTestCases))]
    public void Then_The_Title_Is_Constructed_Correctly(string? searchTerm, int[]? routes, int[]? levels, string? location, bool disabilityConfident, string? expectedTitle)
    {
        // arrange
        var savedSearch = new SavedSearch(
            Guid.NewGuid(),
            DateTime.UtcNow,
            null, null,
            new SearchParameters(
                searchTerm,
                routes?.ToList(),
                10,
                disabilityConfident,
                levels?.ToList(),
                location
            )
        );
        
        // act
        var result = SavedSearchViewModel.From(savedSearch, RouteInfos);

        // assert
        result.Title.Should().Be(expectedTitle);
    }
    
    [TestCase(null, "Across all of England")]
    [TestCase("", "Across all of England")]
    [TestCase("Hull", "Hull")]
    public void Then_The_Location_Is_Set_Correctly(string? location, string? expectedLocation)
    {
        // arrange
        var savedSearch = new SavedSearch(
            Guid.NewGuid(),
            DateTime.UtcNow,
            null, null,
            new SearchParameters("Foo", [1, 2], 10, true, [1, 2], location)
        );
        
        // act
        var result = SavedSearchViewModel.From(savedSearch, RouteInfos);

        // assert
        result.Location.Should().Be(expectedLocation);
    }
}