using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsQuery(Guid CandidateId) : IRequest<GetSubmittedApplicationsQueryResult>;
}
