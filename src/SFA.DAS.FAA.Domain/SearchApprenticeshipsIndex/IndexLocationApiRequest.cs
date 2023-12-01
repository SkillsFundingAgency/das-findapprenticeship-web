using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class IndexLocationApiRequest : IGetApiRequest
{
    private readonly string _searchTerm;

    public IndexLocationApiRequest(string searchTerm) => _searchTerm = searchTerm;

    public string GetUrl => $"searchapprenticeships/indexlocation?locationSearchTerm={_searchTerm}";
}