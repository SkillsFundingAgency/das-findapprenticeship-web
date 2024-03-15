using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class UpdateDateOfBirthApiRequest : IPostApiRequest
{
    private readonly string _govIdentifier;

    public UpdateDateOfBirthApiRequest(string govIdentifier, UpdateDateOfBirthRequestData data)
    {
        _govIdentifier = govIdentifier;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_govIdentifier}/date-of-birth";
}
public class UpdateDateOfBirthRequestData
{
    public required DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
}
