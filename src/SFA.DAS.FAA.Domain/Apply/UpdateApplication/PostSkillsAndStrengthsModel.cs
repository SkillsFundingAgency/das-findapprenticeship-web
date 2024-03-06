using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record PostSkillsAndStrengthsModel
{
    public Guid CandidateId { get; set; }
    public string SkillsAndStrengths { get; set; }
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
