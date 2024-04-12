using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
public class GetAddressesByPostcodeQueryResult
{
    public string? Uprn { get; set; }
    public string? Postcode { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string Town { get; set; } = null!;
    public string? County { get; set; }


    public IEnumerable<GetAddressesByPostcodeApiResponse.AddressListItem>? Addresses { get; set; } = new List<GetAddressesByPostcodeApiResponse.AddressListItem>();

    public static implicit operator GetAddressesByPostcodeQueryResult(GetAddressesByPostcodeApiResponse source)
    {
        return new GetAddressesByPostcodeQueryResult
        {
            Postcode = source.Postcode,
            Addresses = source.Addresses,
            Uprn = source.Uprn
        };
    }
}
