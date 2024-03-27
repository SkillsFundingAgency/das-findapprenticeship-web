using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class CreateUserManuallyEnteredAddressApiRequest : IPostApiRequest
{
    private readonly Guid _candidateId;

    public CreateUserManuallyEnteredAddressApiRequest(Guid candidateId, CreateUserManuallyEnteredAddressApiRequestData data)
    {
        _candidateId = candidateId;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_candidateId}/enter-address";
}
public class CreateUserManuallyEnteredAddressApiRequestData
{
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string TownOrCity { get; set; }
    public string? County { get; set; }
    public string Postcode { get; set; }
}