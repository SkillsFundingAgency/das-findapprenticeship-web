using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;

public record UpdateApplicationModel
{
    public SectionStatus WorkHistorySectionStatus { get; set; }
}