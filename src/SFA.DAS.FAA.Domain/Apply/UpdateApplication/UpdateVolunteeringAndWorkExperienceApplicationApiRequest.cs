using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateVolunteeringAndWorkExperienceApplicationApiRequest
    (
    Guid ApplicationId, 
    Guid CandidateId, 
    UpdateVolunteeringAndWorkHistoryApplicationModel UpdateApplicationModel) 
    : IPostApiRequest
{
    public string PostUrl => $"applications/{ApplicationId}/{CandidateId}/volunteering-and-work-experience";

    public object Data { get; set; } = UpdateApplicationModel;
}
