using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateVolunteeringAndWorkHistoryApplicationModel
{
    public SectionStatus VolunteeringAndWorkExperienceSectionStatus { get; set; }
}
