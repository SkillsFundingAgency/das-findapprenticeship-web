using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class IndexLocationApiRequest : IGetApiRequest
{
    private readonly string _searchTerm;

    public IndexLocationApiRequest(string searchTerm)
    {
        _searchTerm = HttpUtility.UrlEncode(searchTerm);
    }

    public string GetUrl => $"searchapprenticeships?locationSearchTerm={_searchTerm}";
}