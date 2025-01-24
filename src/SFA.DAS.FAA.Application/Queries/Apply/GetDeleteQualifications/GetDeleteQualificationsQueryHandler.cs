using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications;

public class GetDeleteQualificationsQueryHandler(IApiClient apiClient): IRequestHandler<GetDeleteQualificationsQuery, GetDeleteQualificationsQueryResult>
{
    public async Task<GetDeleteQualificationsQueryResult> Handle(GetDeleteQualificationsQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetDeleteQualificationsApiResponse>(
            new GetDeleteQualificationsApiRequest(request.ApplicationId, request.CandidateId,
                request.QualificationType, request.Id));

        return response;
    }
}