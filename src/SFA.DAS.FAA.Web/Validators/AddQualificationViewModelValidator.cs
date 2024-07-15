using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.InputValidation.Fluent.Extensions;

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
        var isApprenticeship = model.GroupTitle.Equals("apprenticeships", StringComparison.CurrentCultureIgnoreCase);
        var isDegree = model.GroupTitle.Equals("degree", StringComparison.CurrentCultureIgnoreCase);
        var isOther = model.GroupTitle.Equals("Other qualifications", StringComparison.CurrentCultureIgnoreCase);
        
        
        if (!isApprenticeship)
        {
            RuleFor(c => c.Name).ValidFreeTextCharacters().WithName(model.SubjectLabel);    
        }
        RuleFor(c => c.AdditionalInformation).ValidFreeTextCharacters().WithName(model.AdditionalInformationLabel);
        RuleFor(c => c.Level).ValidFreeTextCharacters().WithName(model.AdditionalInformationLabel);
        RuleFor(c => c.Grade).ValidFreeTextCharacters().WithName(model.GradeLabel);
        
        When(x => x.IsDeleted is false, () =>
        {
            

            if (isOther)
            {
                RuleFor(x => x.Name)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .WithMessage(model.SubjectErrorMessage);
                
                return;
            }
            
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Continue)
                .NotEmpty()
                .WithMessage(model.SubjectErrorMessage)
                .When(c => c.IsDeleted is null or false);
            
            When(c => !isApprenticeship, () =>
            {
                RuleFor(x => x.Grade)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .WithMessage(model.GradeErrorMessage)
                    .When(c => (c.IsDeleted is null or false 
                                || (model.CanShowLevel )
                                || !string.IsNullOrEmpty(c.AdditionalInformation)
                                || (isDegree && !string.IsNullOrEmpty(c.AdditionalInformation))
                                                              ) && model.GradeErrorMessage != null);
                
                
            });

            When(c => model.ShouldDisplayAdditionalInformationField, () =>
            {
                RuleFor(x => x.AdditionalInformation)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .WithMessage(model.AdditionalInformationErrorMessage);
                
            });
            When(c => model.CanShowLevel, () =>
            {
                RuleFor(x => x.Level)
                    .Cascade(CascadeMode.Continue)
                    .NotEmpty()
                    .NotEqual("select", StringComparer.CurrentCultureIgnoreCase)
                    .WithMessage(model.AdditionalInformationErrorMessage);
                
            });
        });
        
    }
}