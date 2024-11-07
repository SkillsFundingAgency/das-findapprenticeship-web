using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Models.User;

public class WhenCreatingSavedSearchViewModel
{
    private static object[] _titleTestCases =
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
    
    [TestCaseSource(nameof(_titleTestCases))]
    public void Then_The_Title_Is_Constructed_Correctly(string? searchTerm, int[]? routes, int[]? levels, string? location, bool disabilityConfident, string? expectedTitle)
    {
        // arrange
        var routeInfos = new List<RouteInfo>
        {
            new (1, "Route One"),
            new (2, "Route Two"),
            new (3, "Route Three"),
            new (4, "Route Four"),
            new (5, "Route Five"),
        };
        
        var savedSearch = new SavedSearch(
            Guid.NewGuid(),
            DateTime.UtcNow,
            null, null,
            new SearchParameters(
                searchTerm,
                routes?.Select(x => x).ToList(),
                10,
                disabilityConfident,
                levels?.Select(x => x).ToList(),
                location
            )
        );
        
        // act
        var result = SavedSearchViewModel.From(savedSearch, routeInfos);

        // assert
        result.Title.Should().Be(expectedTitle);
    }
}