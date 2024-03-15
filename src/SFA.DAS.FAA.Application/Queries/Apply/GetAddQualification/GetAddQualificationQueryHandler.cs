using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetAddQualification;

public class GetAddQualificationQueryHandler(IApiClient apiClient) : IRequestHandler<GetAddQualificationQuery, GetAddQualificationQueryResult>
{
    public async Task<GetAddQualificationQueryResult> Handle(GetAddQualificationQuery request, CancellationToken cancellationToken)
    {
        var result =
            await apiClient.Get<GetAddQualificationApiResponse>(
                new GetAddQualificationApiRequest(request.QualificationReferenceId,request.ApplicationId));

        return new GetAddQualificationQueryResult
        {
            QualificationType = result.QualificationType
        };
    }
}