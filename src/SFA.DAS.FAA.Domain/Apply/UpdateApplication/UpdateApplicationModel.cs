using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;

public record UpdateApplicationModel
{
    public SectionStatus WorkHistorySectionStatus { get; set; }
}