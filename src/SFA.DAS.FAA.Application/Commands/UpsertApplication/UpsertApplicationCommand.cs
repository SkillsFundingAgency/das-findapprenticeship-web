using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpsertApplication.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpsertApplication
{
    public class UpsertApplicationCommand : IRequest<UpsertApplicationCommandResult>
    {
        public required string VacancyReference { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
        public SectionStatus WorkHistorySectionStatus { get; set; }
    }
}