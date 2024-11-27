using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetSavedSearch;

public record GetSavedSearchQuery(Guid CandidateId, Guid Id) : IRequest<GetSavedSearchQueryResult>;