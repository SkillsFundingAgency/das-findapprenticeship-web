using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateDisabilityConfidenceApplicationModel
{
    public SectionStatus DisabilityConfidenceModelSectionStatus { get; set; }
}
