using SFA.DAS.FAA.Application.Queries.EqualityQuestions;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsModel
    {
        public Guid? ApplicationId { get; init; }
        public GenderIdentity Sex { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public EthnicGroup? SelectedEthnicGroup { get; set; }
        public EthnicGroup EthnicGroup { get; set; }
        public EthnicSubGroup EthnicSubGroup { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }

        public static EqualityQuestionsModel MapFromQueryResult(GetEqualityQuestionsQueryResult queryResult)
        {
            if (queryResult.EqualityQuestions == null)
            {
                throw new InvalidOperationException($"Unable to edit equality questions for candidate");
            }

            return new EqualityQuestionsModel
            {
                EthnicGroup = queryResult.EqualityQuestions.EthnicGroup.GetValueOrDefault(),
                EthnicSubGroup = queryResult.EqualityQuestions.EthnicSubGroup.GetValueOrDefault(),
                IsGenderIdentifySameSexAtBirth = queryResult.EqualityQuestions.IsGenderIdentifySameSexAtBirth,
                OtherEthnicSubGroupAnswer = queryResult.EqualityQuestions.OtherEthnicSubGroupAnswer,
                Sex = queryResult.EqualityQuestions.Sex.GetValueOrDefault()
            };
        }

        public void Apply(EqualityQuestionsGenderViewModel source)
        {
            Sex = (GenderIdentity)Enum.Parse(typeof(GenderIdentity), source.Sex!, true);
            IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth;
        }

        public void Apply(EqualityQuestionsEthnicGroupViewModel source)
        {
            SelectedEthnicGroup = (EthnicGroup)Enum.Parse(typeof(EthnicGroup), source.EthnicGroup!, true);
        }

        public void Apply(EqualityQuestionEthnicSubGroupViewModelBase source)
        {
            var ethnicSubGroup = (EthnicSubGroup)Enum.Parse(typeof(EthnicSubGroup), source.EthnicSubGroup!, true);

            EthnicGroup = source.EthnicGroup;
            EthnicSubGroup = ethnicSubGroup;
            OtherEthnicSubGroupAnswer = ethnicSubGroup is EthnicSubGroup.AnyOtherWhiteBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherAsianBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherBlackAfricanOrCaribbeanBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherMixedBackground
                                                          | ethnicSubGroup is EthnicSubGroup.AnyOtherEthnicGroup
                ? source.OtherEthnicSubGroupAnswer
                : string.Empty;
        }
    }
}