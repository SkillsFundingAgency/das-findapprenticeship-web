using MediatR;

namespace SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
public class CreateSkillsAndStrengthsCommand : IRequest
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string SkillsAndStrengths { get; set; }
}
