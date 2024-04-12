using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetAddressesByPostcodeApiRequest(Guid candidateId, string postcode) : IGetApiRequest
{
    public string GetUrl => $"users/{candidateId}/create-account/select-address?postcode={postcode}";
}
