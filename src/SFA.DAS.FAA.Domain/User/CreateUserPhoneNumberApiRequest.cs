using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class CreateUserPhoneNumberApiRequest : IPostApiRequest
{
    private readonly Guid _candidateId;

    public CreateUserPhoneNumberApiRequest(Guid candidateId, CreateUserPhoneNumberApiRequestData data)
    {
        _candidateId = candidateId;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_candidateId}/create-account/phone-number";
}
public class CreateUserPhoneNumberApiRequestData
{
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}