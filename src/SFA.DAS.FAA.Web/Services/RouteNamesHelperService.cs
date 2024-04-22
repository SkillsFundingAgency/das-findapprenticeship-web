using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Services
{
    public static class RouteNamesHelperService
    {
        public static string GetEqualityFlowEthnicSubGroupRoute(EthnicGroup group)
        {
            return group switch
            {
                EthnicGroup.PreferNotToSay => RouteNames.ApplyApprenticeship.EqualityQuestions
                    .EqualityFlowSummary,
                EthnicGroup.White => RouteNames.ApplyApprenticeship.EqualityQuestions
                    .EqualityFlowEthnicSubGroupWhite,
                EthnicGroup.MixedOrMultiple => RouteNames.ApplyApprenticeship.EqualityQuestions
                    .EqualityFlowEthnicSubGroupMixed,
                EthnicGroup.AsianOrAsianBritish => RouteNames.ApplyApprenticeship.EqualityQuestions
                    .EqualityFlowEthnicSubGroupAsian,
                EthnicGroup.BlackOrAfricanOrCaribbeanOrBlackBritish => RouteNames.ApplyApprenticeship.EqualityQuestions
                    .EqualityFlowEthnicSubGroupBlack,
                EthnicGroup.Other => RouteNames.ApplyApprenticeship.EqualityQuestions
                    .EqualityFlowEthnicSubGroupOther,
                _ => throw new ArgumentOutOfRangeException(nameof(group), group, null)
            };
        }
    }
}
