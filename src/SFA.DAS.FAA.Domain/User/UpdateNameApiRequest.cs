using SFA.DAS.FAA.Domain.Interfaces;
namespace SFA.DAS.FAA.Domain.User
{
    public class UpdateNameApiRequest : IPutApiRequest
    {
        private readonly string _govIdentifier;

        public UpdateNameApiRequest(string govIdentifier, UpdateNameApiRequestData data)
        {
            _govIdentifier = govIdentifier;
            Data = data;
        }
        public object Data { get; set; }
        public string PutUrl => $"users/{_govIdentifier}/add-details";
    }
    public class UpdateNameApiRequestData
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

    }
}