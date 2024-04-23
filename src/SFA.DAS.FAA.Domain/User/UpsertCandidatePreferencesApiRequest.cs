using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class UpsertCandidatePreferencesApiRequest : IPostApiRequest
{
    private readonly Guid _candidateId;

    public UpsertCandidatePreferencesApiRequest(Guid candidateId, UpsertCandidatePreferencesData data)
    {
        _candidateId = candidateId;
        Data = data;
    }
    public object Data { get; set; }
    public string PostUrl => $"users/{_candidateId}/create-account/candidate-preferences";
}
public class UpsertCandidatePreferencesData
{
    public List<CandidatePreference> CandidatePreferences { get; set; }
    public string Email { get; set; }

    public class CandidatePreference
    {
        public Guid PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public string PreferenceHint { get; set; } = null!;
        public List<ContactMethodStatus> ContactMethodsAndStatus { get; set; }
    }

    public class ContactMethodStatus
    {
        public string ContactMethod { get; set; }
        public bool Status { get; set; }
    }
}