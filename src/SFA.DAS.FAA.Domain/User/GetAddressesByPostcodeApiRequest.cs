using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetAddressesByPostcodeApiRequest : IGetApiRequest
{
    private readonly string _postcode;

    public GetAddressesByPostcodeApiRequest(string postcode)
    {
        _postcode = postcode;
    }

    public string GetUrl => $"users/create-account/select-address?postcode={_postcode}";
}
