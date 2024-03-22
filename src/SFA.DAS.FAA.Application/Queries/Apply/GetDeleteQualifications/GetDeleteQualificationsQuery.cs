using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications
{
    public class GetDeleteQualificationsQuery : IRequest<GetDeleteQualificationsQueryResult>
    {
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
        public Guid QualificationType { get; init; }
        public Guid? Id { get; set; }
    }
}
