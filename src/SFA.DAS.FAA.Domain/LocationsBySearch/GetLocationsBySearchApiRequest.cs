using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.LocationsBySearch;
public class GetLocationsBySearchApiRequest : IGetApiRequest
{
    private readonly string _searchTerm;

    public GetLocationsBySearchApiRequest(string searchTerm) => _searchTerm = searchTerm;

    public string GetUrl => $"locations/searchbylocation?searchTerm={_searchTerm}";
}