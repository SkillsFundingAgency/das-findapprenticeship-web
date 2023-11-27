using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
public class GetLocationsByIndexSearchApiRequest : IGetApiRequest
{
    private readonly string _searchTerm;

    public GetLocationsByIndexSearchApiRequest(string searchTerm) => _searchTerm = searchTerm;

    public string GetUrl => $"locations/searchbylocation?searchTerm={_searchTerm}";
}