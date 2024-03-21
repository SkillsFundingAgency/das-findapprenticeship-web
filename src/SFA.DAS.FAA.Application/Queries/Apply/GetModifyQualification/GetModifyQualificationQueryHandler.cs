using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;

public class GetModifyQualificationQueryHandler(IApiClient apiClient) : IRequestHandler<GetModifyQualificationQuery, GetModifyQualificationQueryResult>
{
    public async Task<GetModifyQualificationQueryResult> Handle(GetModifyQualificationQuery request, CancellationToken cancellationToken)
    {
        var result =
            await apiClient.Get<GetModifyQualificationApiResponse>(
                new GetModifyQualificationApiRequest(request.QualificationReferenceId,request.ApplicationId, request.CandidateId, request.QualificationId));

        return new GetModifyQualificationQueryResult
        {
            QualificationType = result.QualificationType,
            Qualifications = result.Qualifications
        };
    }
}