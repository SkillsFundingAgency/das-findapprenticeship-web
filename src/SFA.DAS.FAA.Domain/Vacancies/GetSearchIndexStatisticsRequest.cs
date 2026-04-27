using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Vacancies;

public sealed class GetSearchIndexStatisticsRequest : IGetApiRequest
{
    public string GetUrl => "vacancies/statistics";
}