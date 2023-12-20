using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class GetVacancyDetailsRequestValidatorTest
{
    public const string VacancyReferenceEmpty = "You must include a vacancy reference.";
    public const string VacancyReferenceTooShort = "The vacancy reference must be atleast 10 characters or more.";
    public const string VacancyReferenceNotNumeric = "The vacancy reference must be a numeric & valid value.";

    [TestCase("0", VacancyReferenceTooShort, false)]
    [TestCase("123456789", VacancyReferenceTooShort, false)]
    [TestCase(null, VacancyReferenceEmpty, false)]
    [TestCase("01234567890", null, true)]
    public async Task Validate_VacancyReference(string vacancyReference, string? errorMessage, bool isValid)
    {
        var model = new GetVacancyDetailsRequest
            { VacancyReference = vacancyReference };

        var sut = new GetVacancyDetailsRequestValidator();
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
}