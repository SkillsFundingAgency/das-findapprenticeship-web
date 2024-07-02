namespace SFA.DAS.FAA.Web.Models.User;

public class SettingsViewModel
{
    public string FirstName { get; set; }
    public string MiddleNames { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool IsAddressFromLookup { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public string County { get; set; }
    public string Postcode { get; set; }
    public string? Uprn { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public bool HasAnsweredEqualityQuestions { get; set; }
    public List<CandidatePreference> CandidatePreferences { get; set; }

    public bool UnfinishedApplicationReminders
    {
        get
        {
            return CandidatePreferences.Any(x => x is { Meaning: Application.Constants.Constants.CandidatePreferences.ContactVacancyClosingMeaning, EmailPreference: true });
        }
    }

    public class CandidatePreference 
    {
        public Guid PreferenceId { get; set; }
        public string Meaning { get; set; }
        public string Hint { get; set; }
        public bool EmailPreference { get; set; }
    }
}
