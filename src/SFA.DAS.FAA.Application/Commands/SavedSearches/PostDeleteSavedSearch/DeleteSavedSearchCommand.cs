using MediatR;

namespace SFA.DAS.FAA.Application.Commands.SavedSearches.PostDeleteSavedSearch;

public class DeleteSavedSearchCommand : IRequest
{
    public Guid Id { get; init; }
    public Guid CandidateId { get; init; }
}