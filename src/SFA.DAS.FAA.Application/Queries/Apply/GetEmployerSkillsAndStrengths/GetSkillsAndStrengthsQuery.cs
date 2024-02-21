using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmployerSkillsAndStrengths;
public class GetSkillsAndStrengthsQuery : IRequest<GetSkillsAndStrengthsQueryResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
}
