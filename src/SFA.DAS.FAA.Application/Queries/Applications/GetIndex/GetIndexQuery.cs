using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public Guid CandidateId { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
