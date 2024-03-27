using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class WhenValidatingApplicationSubmittedViewModel
{
    [Test, MoqAutoData]
    public async Task And_No_Radio_Selected_Then_Error(string employerName, string vacancyTitle, Guid applicationId)
    {
        var model = new ApplicationSubmittedViewModel
        {
            AnswerEqualityQuestions = null,
            ApplicationId = applicationId,
            EmployerName = employerName,
            VacancyTitle = vacancyTitle
        };

        var validator = new ApplicationSubmittedViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.AnswerEqualityQuestions)
            .WithErrorMessage("Select if you want to answer the equality questions");
    }
}
