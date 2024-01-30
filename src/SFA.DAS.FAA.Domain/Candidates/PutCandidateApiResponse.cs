namespace SFA.DAS.FAA.Domain.Candidates;
public class PutCandidateApiResponse
{
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
