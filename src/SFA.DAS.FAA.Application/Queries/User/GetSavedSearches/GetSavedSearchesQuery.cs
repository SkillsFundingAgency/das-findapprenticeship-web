using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetSavedSearches;

public class GetSavedSearchesQuery : IRequest<GetSavedSearchesQueryResult>
{
    public Guid CandidateId { get; init; }
}