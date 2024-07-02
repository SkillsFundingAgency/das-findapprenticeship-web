using MediatR;

namespace SFA.DAS.FAA.Application.Commands.CreateAccount.CandidatePreferences;
public class UpsertCandidatePreferencesCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public string CandidateEmail { get; set; }
    public bool UnfinishedApplicationReminders { get; set; }
}