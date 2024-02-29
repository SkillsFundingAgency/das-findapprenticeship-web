using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;

public class UpdateAdditionalQuestionApplicationApiRequest(
    Guid applicationId,
    Guid candidateId,
    UpdateAdditionalQuestionApplicationModel updateApplicationModel)
    : IPostApiRequest
{
    public object Data { get; set; } = updateApplicationModel;

    public string PostUrl => $"applications/{applicationId}/{candidateId}/additional-question";
}