using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    public class RouteNamesHelperServiceTest
    {
        [TestCase(EthnicGroup.PreferNotToSay, RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary)]
        [TestCase(EthnicGroup.AsianOrAsianBritish, RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupAsian)]
        [TestCase(EthnicGroup.BlackOrAfricanOrCaribbeanOrBlackBritish, RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupBlack)]
        [TestCase(EthnicGroup.MixedOrMultiple, RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupMixed)]
        [TestCase(EthnicGroup.Other, RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupOther)]
        [TestCase(EthnicGroup.White, RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicSubGroupWhite)]
        public void FormatEmployerWebsiteUrl(EthnicGroup ethnicGroup, string? expectedRouteName)
        {
            //sut
            var result = RouteNamesHelperService.GetEqualityFlowEthnicSubGroupRoute(ethnicGroup);

            //assert
            result.Should().Be(expectedRouteName);
        }
    }
}
