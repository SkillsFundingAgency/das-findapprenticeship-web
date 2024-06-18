using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class ChangeNotificationPreferencesViewModelValidator : AbstractValidator<ChangeNotificationPreferencesViewModel>
{
    public ChangeNotificationPreferencesViewModelValidator()
    {
        RuleFor(x => x.IsRemindersOpt)
            .NotNull()
            .WithMessage("Select if you want to get reminders about unfinished applications");
    }
}
