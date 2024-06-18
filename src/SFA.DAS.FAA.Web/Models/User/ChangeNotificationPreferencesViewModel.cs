namespace SFA.DAS.FAA.Web.Models.User;

public class ChangeNotificationPreferencesViewModel : ViewModelBase
{
    private bool? _isRemindersOpt;
    public bool? IsRemindersOpt
    {
        get => UnfinishedApplicationReminders;
        set => _isRemindersOpt = value;
    }

    public List<CandidatePreference> CandidatePreferences { get; set; }
    public string BackLink { get; set; }

    private bool UnfinishedApplicationReminders
    {
        get
        {
            return CandidatePreferences.Any(x => x is { Meaning: "A vacancy is closing soon", EmailPreference: true });
        }
    }

    public class CandidatePreference
    {
        public string Meaning { get; set; }

        public bool EmailPreference { get; set; }
    }
}