using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateSkillsAndStrengthsApplicationModel
{
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
