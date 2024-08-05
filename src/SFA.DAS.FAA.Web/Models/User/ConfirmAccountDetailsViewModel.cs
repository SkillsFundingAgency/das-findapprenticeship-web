namespace SFA.DAS.FAA.Web.Models.User;

public class ConfirmAccountDetailsViewModel
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
    public bool UnfinishedApplicationReminders
    {
        get
        {
            return CandidatePreferences !=null && CandidatePreferences.Any(x => x is { Meaning: Application.Constants.Constants.CandidatePreferences.ContactVacancyClosingMeaning, EmailPreference: true });
        }
    }
    public List<CandidatePreference>? CandidatePreferences { get; set; }
    public UserJourneyPath JourneyPath { get; set; }
    public string PageCaption => JourneyPath == UserJourneyPath.AccountFound ? string.Empty : "Create an account";
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.AccountFound ? "Continue" : "Create your account";

    public class CandidatePreference 
    {
        public Guid PreferenceId { get; set; }
        public string Meaning { get; set; }
        public string Hint { get; set; }
        public bool TextPreference { get; set; }
        public bool EmailPreference { get; set; }
    }
}
