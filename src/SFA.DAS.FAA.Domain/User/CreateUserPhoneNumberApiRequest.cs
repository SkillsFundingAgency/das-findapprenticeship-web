using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class CreateUserPhoneNumberApiRequest : IPostApiRequest
{
    private readonly string _govUkIdentifier;

    public CreateUserPhoneNumberApiRequest(string govUkIdentifier, CreateUserPhoneNumberApiRequestData data)
    {
        _govUkIdentifier = govUkIdentifier;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_govUkIdentifier}/phone-number";
}
public class CreateUserPhoneNumberApiRequestData
{
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}