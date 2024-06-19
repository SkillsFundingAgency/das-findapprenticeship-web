using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class NotificationPreferencesViewModelValidator : AbstractValidator<NotificationPreferencesViewModel>
{
    public NotificationPreferencesViewModelValidator()
    {
        RuleFor(x => x.UnfinishedApplicationReminders)
            .NotNull()
            .WithMessage("Select if you want to get reminders about unfinished applications");
    }
}
