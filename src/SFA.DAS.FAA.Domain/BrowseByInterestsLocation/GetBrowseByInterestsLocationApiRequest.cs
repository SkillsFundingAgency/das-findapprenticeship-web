using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.BrowseByInterestsLocation;

public class GetBrowseByInterestsLocationApiRequest : IGetApiRequest
{
    private readonly string _searchTerm;

    public GetBrowseByInterestsLocationApiRequest(string searchTerm)
    {
        _searchTerm = HttpUtility.UrlEncode(searchTerm);
    }

    public string GetUrl => $"searchapprenticeships/browsebyinterestslocation?locationSearchTerm={_searchTerm}";
}