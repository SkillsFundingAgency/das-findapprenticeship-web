namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsModel
    {
        public Guid ApplicationId { get; set; }
        public string? Sex { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }

        public static implicit operator EqualityQuestionsModel(EqualityQuestionsGenderViewModel source)
        {
            return new EqualityQuestionsModel
            {
                ApplicationId = source.ApplicationId,
                Sex = source.Sex,
                IsGenderIdentifySameSexAtBirth = source.IsGenderIdentifySameSexAtBirth
            };
        }
    }
}
