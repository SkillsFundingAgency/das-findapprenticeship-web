using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingSearchModel
{
    [TestCase("<script>test</script>", false)]
    [TestCase("", true)]
    [TestCase("an apprenticeship @! 'Brilliant'", true)]
    [TestCase(null, true)]
    public async Task Then_The_What_Field_Is_Validated(string? inputValue, bool isValid)
    {
        var model = new SearchModel
        {
            WhatSearchTerm = inputValue
        };
        
        var validator = new SearchModelValidator();

        var actual= await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
    }
    [TestCase("<script>test</script>", false)]
    [TestCase("", true)]
    [TestCase("LS12 5RD", true)]
    [TestCase(null, true)]
    public async Task Then_The_Where_Field_Is_Validated(string? inputValue, bool isValid)
    {
        var model = new SearchModel
        {
            WhereSearchTerm = inputValue
        };
        
        var validator = new SearchModelValidator();

        var actual= await validator.ValidateAsync(model);

        actual.IsValid.Should().Be(isValid);
    }
}