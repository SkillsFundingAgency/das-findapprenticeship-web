namespace SFA.DAS.FAA.Web.Models.User;

public class NotificationPreferencesViewModel : ViewModelBase
{
    public List<NotificationPreferenceItemViewModel> NotificationPreferences { get; set; }
}

public class NotificationPreferenceItemViewModel
{
    public Guid PreferenceId { get; set; }
    public string Meaning { get; set; }
    public string Hint { get; set; }
    public bool TextPreference { get; set; }
    public bool EmailPreference { get; set; }
}
