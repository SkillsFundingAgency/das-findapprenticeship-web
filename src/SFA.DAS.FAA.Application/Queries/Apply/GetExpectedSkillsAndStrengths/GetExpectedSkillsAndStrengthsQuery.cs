using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
public class GetExpectedSkillsAndStrengthsQuery : IRequest<GetExpectedSkillsAndStrengthsQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
