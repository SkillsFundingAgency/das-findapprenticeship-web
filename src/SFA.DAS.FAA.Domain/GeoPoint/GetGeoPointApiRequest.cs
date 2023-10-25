using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.GeoPoint;
public class GetGeoPointApiRequest : IGetApiRequest
{
    private readonly string _postCode;

    public GetGeoPointApiRequest(string postCode) => _postCode = postCode;

    public string GetUrl => $"locations/geopoint?postcode={_postCode}";
}