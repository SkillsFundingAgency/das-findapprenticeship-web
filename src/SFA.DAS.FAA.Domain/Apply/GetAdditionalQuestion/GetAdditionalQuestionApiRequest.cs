using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;

public class GetAdditionalQuestionApiRequest(Guid applicationId, Guid candidateId, Guid id) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/additionalquestions?candidateId={candidateId}&id={id}";
}