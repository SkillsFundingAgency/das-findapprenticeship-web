using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CandidatePreferences;
public class UpsertCandidatePreferencesCommand : IRequest<Unit>
{
    public List<NotificationPreferenceItem> NotificationPreferences { get; set; }
    public Guid CandidateId { get; set; }
    public string CandidateEmail { get; set; }
}

public class NotificationPreferenceItem
{
    public Guid PreferenceId { get; set; }
    public string Meaning { get; set; }
    public string Hint { get; set; }
    public bool TextPreference { get; set; }
    public bool EmailPreference { get; set; }
}
