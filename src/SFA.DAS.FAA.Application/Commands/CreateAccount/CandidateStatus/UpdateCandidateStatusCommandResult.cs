using SFA.DAS.FAA.Domain.Candidates;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.CandidateStatus
{
    public record UpdateCandidateStatusCommandResult
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static implicit operator UpdateCandidateStatusCommandResult(PutCandidateApiResponse source)
        {
            return new UpdateCandidateStatusCommandResult
            {
                Id = source.Id,
                Email = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                GovUkIdentifier = source.GovUkIdentifier
            };
        }
    }
}