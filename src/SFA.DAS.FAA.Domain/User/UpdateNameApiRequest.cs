using SFA.DAS.FAA.Domain.Interfaces;
namespace SFA.DAS.FAA.Domain.User
{
    public class UpdateNameApiRequest : IPutApiRequest
    {
        private readonly Guid _candidateId;

        public UpdateNameApiRequest(Guid candidateId, UpdateNameApiRequestData data)
        {
            _candidateId = candidateId;
            Data = data;
        }
        public object Data { get; set; }
        public string PutUrl => $"users/{_candidateId}/create-account/add-details";
    }
    public class UpdateNameApiRequestData
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

    }
}