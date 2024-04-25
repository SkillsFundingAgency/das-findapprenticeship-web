using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class UpdateDateOfBirthApiRequest(Guid candidateId, UpdateDateOfBirthRequestData data) : IPostApiRequest
{
    public object Data { get; set; } = data;
    public string PostUrl => $"users/{candidateId}/create-account/date-of-birth";
}