using MediatR;

namespace CreateAccount.GetCandidatePreferences;
public class GetCandidatePreferencesQuery : IRequest<GetCandidatePreferencesQueryResult>
{
    public Guid CandidateId { get; set; }
}
