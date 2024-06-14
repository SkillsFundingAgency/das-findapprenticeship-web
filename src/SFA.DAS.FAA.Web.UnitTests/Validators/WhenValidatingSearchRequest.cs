using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingSearchRequest
{
    [TestCase("<script>test</script>", false)]
    [TestCase("", true)]
    [TestCase("an apprenticeship @! 'Brilliant'", true)]
    [TestCase(null, true)]
    public async Task Then_The_SearchTerm_Field_Is_Validated(string? inputValue, bool isValid)
    {
        var model = new GetSearchResultsRequest
        {
            SearchTerm = inputValue
        };
        
        var validator = new GetSearchResultsRequestValidator();

        var actual= await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
    }
    [TestCase("<script>test</script>", false)]
    [TestCase("", true)]
    [TestCase("LS12 5RD", true)]
    [TestCase(null, true)]
    public async Task Then_The_Location_Field_Is_Validated(string? inputValue, bool isValid)
    {
        var model = new GetSearchResultsRequest
        {
            Location = inputValue
        };
        
        var validator = new GetSearchResultsRequestValidator();

        var actual= await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
    }
}