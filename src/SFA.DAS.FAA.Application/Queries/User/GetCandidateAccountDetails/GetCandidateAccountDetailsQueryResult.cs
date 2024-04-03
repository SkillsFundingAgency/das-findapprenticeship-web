namespace SFA.DAS.FAA.Application.Queries.User.GetCandidateAccountDetails;
public class GetCandidateAccountDetailsQueryResult
{
    public string FirstName { get; set; }
    public string MiddleNames { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public string County { get; set; }
    public string Postcode { get; set; }
    public List<CandidatePreference> CandidatePreferences { get; set; }

    public class CandidatePreference
    {
        public Guid PreferenceId { get; set; }
        public string Meaning { get; set; }
        public string Hint { get; set; }
        public bool TextPreference { get; set; }
        public bool EmailPreference { get; set; }
    }
}
