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
        When(x => x.IsDeleted is null or false, () =>
        {
            var isApprenticeship = model.GroupTitle.Equals("apprenticeships", StringComparison.CurrentCultureIgnoreCase);
            var isDegree = model.GroupTitle.Equals("degree", StringComparison.CurrentCultureIgnoreCase);
            var isOther = model.GroupTitle.Equals("Other qualifications", StringComparison.CurrentCultureIgnoreCase);

            if (isOther)
            {
                RuleFor(x => x.AdditionalInformation)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .WithMessage(model.AdditionalInformationErrorMessage);
                return;
            }
            
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                .WithMessage(model.SubjectErrorMessage)
                .When(c => !string.IsNullOrEmpty(c.Grade) || c.Id.HasValue 
                                                          || (isApprenticeship && !string.IsNullOrEmpty(c.AdditionalInformation))
                                                          || (isDegree && !string.IsNullOrEmpty(c.AdditionalInformation))
                           || (model.CanShowLevel && !string.IsNullOrEmpty(c.AdditionalInformation)) );
            
            When(c => !isApprenticeship, () =>
            {
                RuleFor(x => x.Grade)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .WithMessage(model.GradeErrorMessage)
                    .When(c => (!string.IsNullOrEmpty(c.Name) || c.Id.HasValue 
                                                              || (model.CanShowLevel && !string.IsNullOrEmpty(c.AdditionalInformation))
                                                              || (isDegree && !string.IsNullOrEmpty(c.AdditionalInformation))
                                                              ) && model.GradeErrorMessage != null);
            });

            When(c => model.ShouldDisplayAdditionalInformationField || model.CanShowLevel, () =>
            {
                RuleFor(x => x.AdditionalInformation)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .WithMessage(model.AdditionalInformationErrorMessage);
            });
        });
        
    }
}