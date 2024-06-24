namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
public class GetCandidatePreferencesQueryResult
{
    public List<CandidatePreference> CandidatePreferences { get; set; } = [];
    public bool? UnfinishedApplicationReminders
    {
        get
        {
            return CandidatePreferences.Any(x => x is { Meaning: Constants.Constants.CandidatePreferences.ContactVacancyClosingMeaning, Preference: null })
                ? null 
                :CandidatePreferences.Any(x => x is { Meaning: Constants.Constants.CandidatePreferences.ContactVacancyClosingMeaning, Preference: true }) ? true : false;
        }
    }
    public class CandidatePreference
    {
        public Guid PreferenceId { get; set; }
        public string Meaning { get; set; }
        public string Hint { get; set; }
        public bool? Preference { get; set; }
    }
}