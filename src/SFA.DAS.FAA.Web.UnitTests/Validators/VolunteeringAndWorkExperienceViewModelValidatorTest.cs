using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class VolunteeringAndWorkExperienceViewModelValidatorTest
{
    private const string NoSelectionErrorMessage = "Select if you want to add any volunteering or work experience";

    [TestCase(NoSelectionErrorMessage, false, null)]
    [TestCase(null, true, true)]
    [TestCase(null, true, false)]
    public async Task Validate_VolunteeringAndWorkExperience(string? errorMessage, bool isValid, bool? doYouWantToAddAnyVolunteeringOrWorkExperience)
    {
        var model = new VolunteeringAndWorkExperienceViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            BackLinkUrl = "",
            DoYouWantToAddAnyVolunteeringOrWorkExperience = doYouWantToAddAnyVolunteeringOrWorkExperience
        };

        var sut = new VolunteeringAndWorkExperienceViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.DoYouWantToAddAnyVolunteeringOrWorkExperience)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.DoYouWantToAddAnyVolunteeringOrWorkExperience);
        }
    }
}
