using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQuery(Guid CandidateId, ApplicationStatus Status)
        : IRequest<GetApplicationsCountQueryResult>;
}