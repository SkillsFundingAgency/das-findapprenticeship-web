using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePreferences;
public class GetCandidatePreferencesQuery : IRequest<GetCandidatePreferencesQueryResult>
{
    public Guid CandidateId { get; set; }
}
