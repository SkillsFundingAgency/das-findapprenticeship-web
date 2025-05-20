using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

[TestFixture]
public class WhenValidatingSearchRequest
{
    [Test]
    [MoqInlineAutoData("<script>test</script>", false)]
    [MoqInlineAutoData("", true)]
    [MoqInlineAutoData("an apprenticeship @! 'Brilliant'", true)]
    [MoqInlineAutoData(null, true)]
    public async Task Then_The_SearchTerm_Field_Is_Validated(
        string? inputValue,
        bool isValid,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        var model = new GetSearchResultsRequest
        {
            SearchTerm = inputValue
        };
        
        var actual= await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
    }

    [Test]
    [MoqInlineAutoData("<script>test</script>", false)]
    [MoqInlineAutoData("", true)]
    [MoqInlineAutoData("LS12 5RD", true)]
    [MoqInlineAutoData(null, true)]
    public async Task Then_The_Location_Field_Is_Validated(
        string? inputValue,
        bool isValid,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        var model = new GetSearchResultsRequest
        {
            Location = inputValue
        };
        var actual= await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
    }

    [Test, MoqAutoData]
    public void Valid_Request_Passes_Validation(
        GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        // Arrange
        model.SearchTerm = "Developer";
        model.Location = "London";
        model.RouteIds = ["1", "2"];
        model.LevelIds = ["3"];
        model.Distance = 10;
        model.PageNumber = 1;
        model.Sort = "AgeAsc";

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test, MoqAutoData]
    public void Invalid_RouteId_Should_Have_Error(GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        model.RouteIds = ["abc"];

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor("RouteIds[0]");
    }

    [Test, MoqAutoData]
    public void Invalid_LevelId_Should_Have_Error(
        GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        model.LevelIds = ["xyz"];

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor("LevelIds[0]");
    }

    [Test, MoqAutoData]
    public void Location_Too_Long_Should_Have_Error(
        GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        model.Location = new string('a', 101);

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Location);
    }

    [Test, MoqAutoData]
    public void Distance_Invalid_Should_Have_Error(
        GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        model.Distance = 99;

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Distance);
    }

    [Test, MoqAutoData]
    public void PageNumber_Less_Than_One_Should_Have_Error(
        GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        model.PageNumber = 0;

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Test, MoqAutoData]
    public void Sort_Invalid_Should_Have_Error(
        GetSearchResultsRequest model,
        [Greedy] GetSearchResultsRequestValidator validator)
    {
        model.Sort = "NotAValidSort";

        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Sort);
    }
}