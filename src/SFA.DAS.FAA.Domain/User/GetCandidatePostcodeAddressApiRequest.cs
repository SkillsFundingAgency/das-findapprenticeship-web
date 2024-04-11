using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetCandidatePostcodeAddressApiRequest : IGetApiRequest
{
    private readonly string _postcode;

    public GetCandidatePostcodeAddressApiRequest(string postcode)
    {
        _postcode = postcode;
    }

    public string GetUrl => $"users/postcode-address?postcode={_postcode}";
}
