using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetIndex
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
