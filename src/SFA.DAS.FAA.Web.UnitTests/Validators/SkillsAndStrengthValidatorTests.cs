using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class SkillsAndStrengthValidatorTests
{
    [Test, MoqInlineAutoData(true, "test skills and strengths")]
    [MoqInlineAutoData(false, "test skills and strengths")]
    public async Task And_Input_Is_Valid_Then_No_Model_Errors(
        bool isSectionComplete,
        string skillsAndStrengths,
        SkillsAndStrengthsViewModel model)
    {
        model.IsSectionComplete = isSectionComplete;
        model.SkillsAndStrengths = skillsAndStrengths;

        var sut = new SkillsAndStrengthsViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        result.IsValid.Should().BeTrue();
    }

    [Test, MoqInlineAutoData("test skills and strengths")]
    public async Task And_Is_Section_Complete_Is_Null_Then_Model_Error(
        string skillsAndStrengths,
        SkillsAndStrengthsViewModel model)
    {
        model.IsSectionComplete = null;
        model.SkillsAndStrengths = skillsAndStrengths;

        var sut = new SkillsAndStrengthsViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("IsSectionComplete");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo("Select if you have finished this section");
        }
    }

    [Test, MoqInlineAutoData("randomly generated words haley silvery normal broadband sly angry passive high-pitched heinous advisable madeline baronetcy following expressionless colonnade  civilian neutral ripped inexperienced avenue sleeveless unfriendly livid sorry understandable alabama varied earthworks dogged petrified high-energy clara disturbed how-to regina cherokee oubliette admitting metaphorical armored kate outward belligerent spent motorized austin gas station deadly slower transnational feasible aurelia underground retreat rachel sick consulting worn rectangular taxable well-educated irreverent nearest fortunate wet smashing illuminated new bridal mammalian cheap transformative unwed improving flipping randomly generated words haley silvery normal broadband sly angry passive high-pitched heinous advisable madeline baronetcy following expressionless colonnade  civilian neutral ripped inexperienced avenue sleeveless unfriendly livid sorry understandable alabama varied earthworks dogged petrified high-energy clara disturbed how-to regina cherokee oubliette admitting metaphorical armored kate outward belligerent spent motorized austin gas station deadly slower transnational feasible aurelia underground retreat rachel sick consulting worn rectangular taxable well-educated irreverent nearest fortunate wet smashing illuminated new bridal mammalian cheap transformative unwed improving flipping  kara scarlett removable interim unionized correct honest hideous four-day gwendolyn stronghold hardened cora louisville deep-seated naturalistic bonnie productive excited cracked strict proximal hour-long skillful null randomly generated words haley silvery normal broadband sly angry passive high-pitched heinous advisable madeline baronetcy following expressionless colonnade  civilian neutral ripped inexperienced avenue sleeveless unfriendly livid sorry understandable alabama varied earthworks dogged petrified high-energy clara disturbed how-to regina cherokee oubliette admitting metaphorical armored kate outward belligerent spent motorized austin gas station deadly slower transnational feasible aurelia underground retreat rachel sick consulting worn rectangular taxable well-educated irreverent nearest fortunate wet smashing illuminated randomly generated words haley silvery normal broadband sly angry passive high-pitched heinous advisable madeline baronetcy following expressionless colonnade  civilian neutral ripped inexperienced avenue sleeveless unfriendly livid sorry understandable alabama varied earthworks dogged petrified high-energy clara disturbed how-to regina cherokee oubliette admitting metaphorical armored kate outward belligerent spent motorized austin gas station deadly slower transnational feasible aurelia underground retreat rachel sick consulting worn rectangular taxable well-educated irreverent nearest fortunate wet smashing illuminated")]
    public async Task And_SkillsAndStrengths_Has_Too_Many_Words_Then_Model_Error(
        string skillsAndStrengths,
        SkillsAndStrengthsViewModel model)
    {
        model.SkillsAndStrengths = skillsAndStrengths;

        var sut = new SkillsAndStrengthsViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("SkillsAndStrengths");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo("Your skills and strengths must be 300 words or less");
        }
    }

    [Test, MoqAutoData]
    public async Task And_SkillsAndStrengths_Has_No_Words_And_Section_Complete_Then_Model_Error(SkillsAndStrengthsViewModel model)
    {
        model.SkillsAndStrengths = null;

        var sut = new SkillsAndStrengthsViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("SkillsAndStrengths");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo("Enter your skills and strengths - you must enter an answer before making this section complete");
        }
    }
}
