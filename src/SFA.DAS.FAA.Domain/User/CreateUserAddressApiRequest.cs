using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class CreateUserAddressApiRequest : IPostApiRequest
{
    private readonly string _govIdentifier;

    public CreateUserAddressApiRequest(string govIdentifier, CreateUserAddressApiRequestData data)
    {
        _govIdentifier = govIdentifier;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_govIdentifier}/select-address";
}
public class CreateUserAddressApiRequestData
{
    public string Email { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
    public string Uprn { get; set; }
}