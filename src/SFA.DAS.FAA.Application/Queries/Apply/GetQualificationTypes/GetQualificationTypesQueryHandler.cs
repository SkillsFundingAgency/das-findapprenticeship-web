using MediatR;
using SFA.DAS.FAA.Domain.Apply.Qualifications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;

public class GetQualificationTypesQueryHandler(IApiClient apiClient) : IRequestHandler<GetQualificationTypesQuery, GetQualificationTypesQueryResponse>
{
    public async Task<GetQualificationTypesQueryResponse> Handle(GetQualificationTypesQuery request, CancellationToken cancellationToken)
    {
        var results = await apiClient.Get<GetQualificationTypesApiResponse>(new GetQualificationTypesApiRequest(request.ApplicationId));

        return new GetQualificationTypesQueryResponse
        {
            HasAddedQualifications = results.HasAddedQualifications,
            QualificationTypes = results.QualificationTypes.OrderBy(c => c.Order).ToList()
        };
    }
}