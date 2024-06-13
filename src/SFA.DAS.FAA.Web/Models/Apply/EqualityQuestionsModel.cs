using SFA.DAS.FAA.Application.Queries.EqualityQuestions;
using SFA.DAS.FAA.Domain.Enums;
using static SFA.DAS.FAA.Web.Infrastructure.RouteNames.ApplyApprenticeship;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsModel
    {
        public Guid? ApplicationId { get; init; }
        public GenderIdentity Sex { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
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
            EthnicGroup = (EthnicGroup)Enum.Parse(typeof(EthnicGroup), source.EthnicGroup!, true);
        }
    }
}