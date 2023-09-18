using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Vacancies;

public class GetVacanciesApiRequest : IGetApiRequest
{
    private readonly int _total;

    public GetVacanciesApiRequest(string baseurl)
    {
        BaseUrl = baseurl;
    }

    public string BaseUrl { get; }
    public string GetUrl => $"{BaseUrl}/searchapprentices";
}