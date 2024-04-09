using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class CreateUserAddressApiRequest : IPostApiRequest
{
    private readonly Guid _candidateId;

    public CreateUserAddressApiRequest(Guid candidateId, CreateUserAddressApiRequestData data)
    {
        _candidateId = candidateId;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_candidateId}/create-account/select-address";
}
public class CreateUserAddressApiRequestData
{
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
}