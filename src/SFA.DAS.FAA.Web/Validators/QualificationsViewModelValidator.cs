using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class QualificationsViewModelValidator : AbstractValidator<QualificationsViewModel>
    {
        public QualificationsViewModelValidator()
        {
            RuleFor(x => x.DoYouWantToAddAnyQualifications)
                .NotNull()
                .WithMessage("Select if you want to add any qualifications ");
        }
    }
}
