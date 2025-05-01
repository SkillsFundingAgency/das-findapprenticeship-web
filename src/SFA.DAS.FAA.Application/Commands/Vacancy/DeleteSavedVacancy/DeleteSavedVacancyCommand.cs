using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommand : IRequest<Unit>
    {
        public Guid CandidateId { get; init; }
        public required string VacancyId { get; set; }
        public bool DeleteAllByVacancyReference { get; set; }
    }
}