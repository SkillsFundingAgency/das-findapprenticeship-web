namespace SFA.DAS.FAA.Domain.Candidates;
public class PutCandidateApiResponse
{
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public UserStatus Status { get; set; }
    public bool IsEmailAddressMigrated { get; set; }
}