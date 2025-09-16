using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class EqualityQuestionsGenderViewModel : ViewModelBase
{
    public Guid? ApplicationId { get; init; }
    public bool IsEdit { get; set; }
    public string? Sex { get; set; }
    public string? IsGenderIdentifySameSexAtBirth { get; set; }
    public string BackLinkRoute => IsEdit || !ApplicationId.HasValue
        ? RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary
        : RouteNames.ApplyApprenticeship.ApplicationSubmitted;

    public static implicit operator EqualityQuestionsGenderViewModel(EqualityQuestionsModel source)
    {
        return new EqualityQuestionsGenderViewModel
        {
            ApplicationId = source.ApplicationId,
            Sex = source.Sex.StringValue(),
            IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth
        };
    }
}