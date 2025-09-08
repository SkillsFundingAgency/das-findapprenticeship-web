using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class EqualityQuestionsEthnicGroupViewModel : ViewModelBase
{
    public Guid? ApplicationId { get; init; }
    public string? EthnicGroup { get; set; }
    public bool IsEdit { get; set; }
    public string BackLinkRoute => IsEdit
        ? RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary
        : RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender;

    public static implicit operator EqualityQuestionsEthnicGroupViewModel(EqualityQuestionsModel source)
    {
        return new EqualityQuestionsEthnicGroupViewModel
        {
            ApplicationId = source.ApplicationId,
            EthnicGroup = source.SelectedEthnicGroup.HasValue
                ? ((int)source.SelectedEthnicGroup).ToString()
                : ((int)source.EthnicGroup).ToString()
        };
    }
}