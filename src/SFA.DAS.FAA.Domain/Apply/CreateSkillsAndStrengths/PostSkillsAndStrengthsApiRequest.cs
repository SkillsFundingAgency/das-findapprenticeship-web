using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.CreateSkillsAndStrengths;
public class PostSkillsAndStrengthsApiRequest(Guid applicationId, PostSkillsAndStrengthsApiRequest.PostCreateSkillsAndStrengthsRequestData body) : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/skillsandstrengths";
    public object Data { get; set; } = body;

    public class PostCreateSkillsAndStrengthsRequestData
    {
        public Guid CandidateId { get; set; }
        public string SkillsAndStrengths { get; set; }
    }
}
