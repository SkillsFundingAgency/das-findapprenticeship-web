using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsSummaryViewModel
    {
        public Guid ApplicationId { get; init; }
        public string? Sex { get; init; }
        public string? IsGenderIdentifySameSexAtBirth { get; init; }
        public string? EthnicGroup { get; init; }

        public static implicit operator EqualityQuestionsSummaryViewModel(EqualityQuestionsModel source)
        {
            return new EqualityQuestionsSummaryViewModel
            {
                ApplicationId = source.ApplicationId,
                Sex = source.Sex.GetDescription(),
                IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth,
                EthnicGroup = GetEthnicGroupDescription(source.EthnicGroup, source.EthnicSubGroup, source.OtherEthnicSubGroupAnswer)
            };
        }

        private static string GetEthnicGroupDescription(EthnicGroup group, EthnicSubGroup subGroup, string? otherEthnicSubGroupAnswer)
        {
            switch (group)
            {
                case Domain.Enums.EthnicGroup.PreferNotToSay:
                    return $"{group.GetDescription()}";
                case Domain.Enums.EthnicGroup.Other when subGroup is EthnicSubGroup.Arab:
                    return $"{subGroup.GetDescription()}";
                case Domain.Enums.EthnicGroup.White when subGroup is EthnicSubGroup.AnyOtherWhiteBackground:
                case Domain.Enums.EthnicGroup.AsianOrAsianBritish when subGroup is EthnicSubGroup.AnyOtherAsianBackground:
                    return !string.IsNullOrEmpty(otherEthnicSubGroupAnswer) ? otherEthnicSubGroupAnswer : "Asian or Asian British background (Any other Asian background)";
                case Domain.Enums.EthnicGroup.MixedOrMultiple when subGroup is EthnicSubGroup.AnyOtherMixedBackground:
                    return !string.IsNullOrEmpty(otherEthnicSubGroupAnswer) ? otherEthnicSubGroupAnswer : "Mixed background (Any other mixed background)";
                case Domain.Enums.EthnicGroup.BlackOrAfricanOrCaribbeanOrBlackBritish when subGroup is EthnicSubGroup.AnyOtherBlackAfricanOrCaribbeanBackground:
                    return !string.IsNullOrEmpty(otherEthnicSubGroupAnswer) ? otherEthnicSubGroupAnswer : "Black, African, Caribbean or Black British background (Any other Black, African, Caribbean or Black British background)";
                case Domain.Enums.EthnicGroup.Other when subGroup is EthnicSubGroup.AnyOtherEthnicGroup:
                    return !string.IsNullOrEmpty(otherEthnicSubGroupAnswer) ? otherEthnicSubGroupAnswer : "Other ethnic background (Any other ethnic background)";
                default:
                    return $"{group.GetDescription()} ({subGroup.GetDescription()})";
            }
        }
    }
}