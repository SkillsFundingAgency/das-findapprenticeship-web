using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetQualifications
{
    public class GetQualificationsQuery : IRequest<GetQualificationsQueryResult>
    {
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
    }

    public class GetQualificationsQueryResult
    {
        public bool? IsSectionCompleted { get; set; }

        public List<Qualification> Qualifications { get; set; } = new List<Qualification>();

        public class Qualification
        {
        }
    }

    public class GetQualificationsQueryHandler : IRequestHandler<GetQualificationsQuery, GetQualificationsQueryResult>
    {
        public Task<GetQualificationsQueryResult> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
