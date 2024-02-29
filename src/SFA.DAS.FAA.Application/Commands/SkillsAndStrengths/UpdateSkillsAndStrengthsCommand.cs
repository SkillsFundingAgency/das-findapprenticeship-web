using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
public class UpdateSkillsAndStrengthsCommand : IRequest<UpdateSkillsAndStrengthsCommandResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string SkillsAndStrengths { get; set; }
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
