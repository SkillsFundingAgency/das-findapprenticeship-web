using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetIndex;

public class GetIndexQueryHandler(IApiClient apiClient) : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
{
    public async Task<GetIndexQueryResult> Handle(GetIndexQuery query, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetIndexApiResponse>(new GetIndexApiRequest(query.VacancyReference));
        return (GetIndexQueryResult) response;
    }
}