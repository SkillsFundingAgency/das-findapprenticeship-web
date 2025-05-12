using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class AddEmploymentLocationsViewModelValidator : AbstractValidator<AddEmploymentLocationsViewModel>
    {
        public AddEmploymentLocationsViewModelValidator()
        {
            RuleFor(x => x.SelectedAddressIds)
                .NotEmpty()
                .WithMessage("Select where you want to apply for");
        }
    }
}