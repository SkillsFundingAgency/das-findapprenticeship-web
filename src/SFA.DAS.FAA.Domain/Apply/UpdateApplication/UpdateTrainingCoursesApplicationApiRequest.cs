using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record UpdateTrainingCoursesApplicationApiRequest
(
    Guid ApplicationId,
    Guid CandidateId,
    UpdateTrainingCoursesApplicationModel UpdateApplicationModel)
    : IPostApiRequest
{
    public object Data { get; set; } = UpdateApplicationModel;

    public string PostUrl => $"applications/{ApplicationId}/{CandidateId}/training-courses";
}