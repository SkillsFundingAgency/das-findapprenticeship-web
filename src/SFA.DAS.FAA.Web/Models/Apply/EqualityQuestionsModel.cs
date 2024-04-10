using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsModel
    {
        public Guid ApplicationId { get; init; }
        public GenderIdentity Sex { get; init; }
        public string? IsGenderIdentifySameSexAtBirth { get; init; }
        public EthnicGroup EthnicGroup { get; set; }

        public static implicit operator EqualityQuestionsModel(EqualityQuestionsGenderViewModel source)
        {
            return new EqualityQuestionsModel
            {
                ApplicationId = source.ApplicationId,
                Sex = (GenderIdentity)Enum.Parse(typeof(GenderIdentity), source.Sex!, true),
                IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth
            };
        }
    }
}
