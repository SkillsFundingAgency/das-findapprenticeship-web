using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication
{
    public record UpdateApplicationCommand : IRequest<UpdateApplicationCommandResult>
    {
        public required string VacancyReference { get; init; }
        public Guid ApplicationId { get; init; }
        public Guid CandidateId { get; init; }
        public SectionStatus WorkHistorySectionStatus { get; init; }
    }
}