using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.EqualityQuestions;

public class GetEqualityQuestionsApiResponse
{
    public EqualityQuestionsItem? EqualityQuestions { get; set; }
    public class EqualityQuestionsItem
    {
        public GenderIdentity? Sex { get; set; }
        public EthnicGroup? EthnicGroup { get; set; }
        public EthnicSubGroup? EthnicSubGroup { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
        public string? OtherEthnicSubGroupAnswer { get; set; }
    }
}