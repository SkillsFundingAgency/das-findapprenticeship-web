using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class NotificationPreferencesViewModelValidatorTest
{
    private const string NoSelectionErrorMessage = "Select if you want to get reminders about unfinished applications";

    [TestCase(NoSelectionErrorMessage, false, null)]
    [TestCase(null, true, true)]
    [TestCase(null, true, false)]
    public async Task Validate_NotificationPreferencesViewModel(string? errorMessage, bool isValid, bool? unfinishedApplicationReminders)
    {
        var model = new NotificationPreferencesViewModel()
        {
            UnfinishedApplicationReminders = unfinishedApplicationReminders
        };

        var sut = new NotificationPreferencesViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.UnfinishedApplicationReminders)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.UnfinishedApplicationReminders);
        }
    }
}