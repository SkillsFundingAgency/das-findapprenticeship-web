using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Apply
{
    public class ApplyCommand : IRequest<ApplyCommandResponse>
    {
        public Guid CandidateId { get; set; }
        public string VacancyReference { get; set; }
    }
}
