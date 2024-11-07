using MediatR;

namespace SFA.DAS.FAA.Application.Queries.SavedSearches.GetSavedSearches;

public class GetSavedSearchesQuery : IRequest<GetSavedSearchesQueryResult>
{
    public Guid CandidateId { get; init; }
}