using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommand : IRequest<Unit>
    {
        public Guid CandidateId { get; init; }
        public required string VacancyReference { get; set; }
    }
}