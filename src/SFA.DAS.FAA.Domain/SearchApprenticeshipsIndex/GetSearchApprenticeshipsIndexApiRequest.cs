using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;

public class GetSearchApprenticeshipsIndexApiRequest : IGetApiRequest
{
    private readonly string? _location;

    public GetSearchApprenticeshipsIndexApiRequest(string? location)
    {
        _location = location;
    }
    public string GetUrl => $"searchapprenticeships?locationSearchTerm={HttpUtility.UrlEncode(_location)}";
}