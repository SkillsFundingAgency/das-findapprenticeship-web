using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class GetVacancyDetailsRequestValidatorTest
{
    private const string VacancyReferenceEmpty = "You must include a vacancy reference.";
    private const string VacancyReferenceTooShort = "The vacancy reference must be atleast 7 characters or more.";

    [TestCase("0", VacancyReferenceTooShort, false)]
    [TestCase("123456", VacancyReferenceTooShort, false)]
    [TestCase(null, VacancyReferenceEmpty, false)]
    [TestCase("01234567", "", true)]
    [TestCase("VAC0123456789", "", true)]
    [TestCase("VACC9224-24-0351", "", true, Description = "Test case to cover NHS Vacancy reference")]
    public async Task Validate_VacancyReference(string vacancyReference, string? errorMessage, bool isValid)
    {
        var model = new GetVacancyDetailsRequest
            {VacancyReference = vacancyReference};

        var sut = new GetVacancyDetailsRequestValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.VacancyReference)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.VacancyReference);
        }
    }
}