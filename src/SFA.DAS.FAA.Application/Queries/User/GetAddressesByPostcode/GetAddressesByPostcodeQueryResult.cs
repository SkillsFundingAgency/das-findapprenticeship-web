using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
public class GetAddressesByPostcodeQueryResult
{
    public IEnumerable<GetAddressesByPostcodeApiResponse.AddressListItem> Addresses
    {
        get;
        set;
    } = new List<GetAddressesByPostcodeApiResponse.AddressListItem>();
}
