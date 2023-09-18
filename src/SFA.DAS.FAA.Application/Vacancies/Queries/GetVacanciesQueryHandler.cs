using MediatR;
using SFA.DAS.FAA.Domain.Configuration;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Vacancies;

namespace SFA.DAS.FAA.Application.Vacancies.Queries;

public class GetVacanciesQueryHandler : IRequestHandler<GetVacanciesQuery, GetVacanciesResult>
{
    private readonly IApiClient _apiClient;
    private readonly FindAnApprenticeshipApi _config;

    public async Task<GetVacanciesResult> Handle(GetVacanciesQuery query, CancellationToken cancellationToken)
    {
        var request = new GetVacanciesApiRequest(_config.BaseUrl);
        var response = await _apiClient.Get<Vacancy>(request);
        return new GetVacanciesResult
        {
            Total = response.Total,
        };
    }
}