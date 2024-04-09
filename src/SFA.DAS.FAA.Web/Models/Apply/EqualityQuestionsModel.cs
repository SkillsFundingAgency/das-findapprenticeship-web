using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class EqualityQuestionsModel
    {
        public Guid ApplicationId { get; set; }
        public string? Sex { get; set; }
        public string? IsGenderIdentifySameSexAtBirth { get; set; }
    }
}
