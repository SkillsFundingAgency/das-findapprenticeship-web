using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy
{
    public record SaveVacancyCommand : IRequest<SaveVacancyCommandResult>
    {
        public Guid CandidateId { get; init; }
        public required string VacancyReference { get; init; }
    }
}