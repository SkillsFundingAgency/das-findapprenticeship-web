using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView
{
    public record GetApplicationViewQuery : IRequest<GetApplicationViewQueryResult>
    {
        public Guid ApplicationId { get; init; }
        public Guid? CandidateId { get; init; }
    }
}
