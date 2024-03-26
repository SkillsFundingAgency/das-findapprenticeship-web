using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class AddQualificationViewModelValidator : AbstractValidator<AddQualificationViewModel>
{
    public AddQualificationViewModelValidator()
    {
        RuleForEach(x => x.Subjects).SetValidator(x=> new SubjectViewModelValidator(new QualificationDisplayTypeViewModel(x.QualificationType,x.QualificationReferenceId)));
    }
}
public class SubjectViewModelValidator : AbstractValidator<SubjectViewModel>
{
    public SubjectViewModelValidator()
    {
    }
    public SubjectViewModelValidator(QualificationDisplayTypeViewModel model)
    {
        RuleFor(x => x.Name).Cascade(CascadeMode.Continue).NotEmpty()
            .WithMessage(model.SubjectErrorMessage)
            .When(c => (!string.IsNullOrEmpty(c.Grade) || c.Id.HasValue) && model.SubjectErrorMessage != null);
        RuleFor(x => x.Grade).Cascade(CascadeMode.Continue).NotEmpty()
            .WithMessage(model.GradeErrorMessage)
            .When(c => (!string.IsNullOrEmpty(c.Name) || c.Id.HasValue) && model.GradeErrorMessage != null);

        When(c => model.ShouldDisplayAdditionalInformationField, () =>
        {
            RuleFor(x => x.AdditionalInformation).Cascade(CascadeMode.Continue).NotEmpty()
                .WithMessage(model.AdditionalInformationErrorMessage);
        });
        
        
    }
}