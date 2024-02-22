using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.SkillsAndStrengths;
public class UpdateSkillsAndStrengthsApplicationCommand : IRequest<UpdateSkillsAndStrengthsApplicationCommandResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public SectionStatus SkillsAndStrengthsSectionStatus { get; set; }
}
