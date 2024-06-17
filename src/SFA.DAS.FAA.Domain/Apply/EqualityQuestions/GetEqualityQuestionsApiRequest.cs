using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.EqualityQuestions
{
    public class GetEqualityQuestionsApiRequest(Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"equalityQuestions?candidateId={CandidateId}";
    }
}
