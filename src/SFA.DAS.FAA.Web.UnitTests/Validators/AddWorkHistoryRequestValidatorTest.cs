using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class AddWorkHistoryRequestValidatorTest
{
    private const string VacancyReferenceEmpty = "You must include a vacancy reference.";
    private const string VacancyReferenceTooShort = "The vacancy reference must be atleast 10 characters or more.";
    private const string VacancyReferenceNotNumeric = "The vacancy reference must be a numeric & valid value.";
    private const string VacancyNotSelectAnyJobs = "Select if you want to add any jobs";

    [TestCase("0", VacancyReferenceTooShort, false)]
    [TestCase("123456789", VacancyReferenceTooShort, false)]
    [TestCase(null, VacancyReferenceEmpty, false)]
    [TestCase("abc", VacancyReferenceNotNumeric, false)]
    [TestCase("01234567890", null, true)]
    public async Task Validate_VacancyReference(string vacancyReference, string? errorMessage, bool isValid)
    {
        var model = new AddWorkHistoryRequest
        {
            VacancyReference = vacancyReference,
            ApplicationId = Guid.NewGuid(),
            AddJob = "Yes"
        };

        var sut = new AddWorkHistoryRequestValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.VacancyReference)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }

    [TestCase(null, VacancyNotSelectAnyJobs, false)]
    [TestCase("Yes", null, true)]
    [TestCase("No", null, true)]
    public async Task Validate_AddJob(string addJob, string? errorMessage, bool isValid)
    {
        var model = new AddWorkHistoryRequest
        {
            VacancyReference = "01234567890",
            ApplicationId = Guid.NewGuid(),
            AddJob = addJob
        };

        var sut = new AddWorkHistoryRequestValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.AddJob)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}