using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetIndex;

public class GetIndexQueryHandler : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetIndexQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetIndexQueryResult> Handle(GetIndexQuery query, CancellationToken cancellationToken)
    {
        var response = await _apiClient.Get<GetIndexApiResponse>(new GetIndexApiRequest(query.VacancyReference, query.ApplicantEmailAddress));
        return response;
    }
}