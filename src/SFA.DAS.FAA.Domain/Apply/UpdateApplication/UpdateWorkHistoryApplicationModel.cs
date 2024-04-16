using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;

public record UpdateWorkHistoryApplicationModel
{
    public SectionStatus WorkHistorySectionStatus { get; set; }
}