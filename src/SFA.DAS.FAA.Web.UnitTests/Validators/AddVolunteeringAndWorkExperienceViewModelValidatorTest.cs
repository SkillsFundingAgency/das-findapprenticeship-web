using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class AddVolunteeringAndWorkExperienceViewModelValidatorTest
{
    private const string CompanyNameErrorMessage = "Enter the company or organisation for this volunteering or work experience";
    private const string JobDescriptionErrorMessage = "Enter what you did for this volunteering or work experience";
    private const string JobDescriptionMaxLengthErrorMessage = "What you did must be 100 words or less";
    private const string StartDateErrorMessage = "Enter a real date for the start date";
    private const string StartDateIsInThePastErrorMessage = "The start date must be in the past";
    private const string IsCurrentJobErrorMessage = "Select if you’re still doing this volunteering or work experience";
    private const string EndDateErrorMessage = "Enter a real date for the end date";
    private const string EndDateIsInThePastErrorMessage = "The end date must be in the past";
    private const string EndDateMustBeGreaterThanStartDate = "The end date must be greater than start date";

    [Test, MoqInlineAutoData("", "", null, null)]
    public async Task Validate_VolunteeringAndWorkExperience(
        string companyName,
        string description,
        MonthYearDate? startDate,
        bool? currentRole,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var model = new AddVolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = Guid.NewGuid(),
            CompanyName = companyName,
            Description = description,
            StartDate = startDate,
            IsCurrentRole = currentRole
        };

        var sut = new AddVolunteeringAndWorkExperienceViewModelValidator(dateTimeService.Object);
        var result = await sut.TestValidateAsync(model);
        
        result.ShouldHaveValidationErrorFor(c => c.CompanyName).WithErrorMessage(CompanyNameErrorMessage);
        result.ShouldHaveValidationErrorFor(c => c.Description).WithErrorMessage(JobDescriptionErrorMessage);
        result.ShouldHaveValidationErrorFor(c => c.StartDate).WithErrorMessage(StartDateErrorMessage);
        result.ShouldHaveValidationErrorFor(c => c.IsCurrentRole).WithErrorMessage(IsCurrentJobErrorMessage);
    }
}