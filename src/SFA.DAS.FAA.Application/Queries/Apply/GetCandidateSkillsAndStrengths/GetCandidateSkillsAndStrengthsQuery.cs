using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
public class GetCandidateSkillsAndStrengthsQuery : IRequest<GetCandidateSkillsAndStrengthsQueryResult>
{
    public Guid CandidateId { get; set; }
    public Guid ApplicationId { get; set; }
}
