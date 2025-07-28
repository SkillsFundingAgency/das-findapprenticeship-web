using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Applications.Delete;

public record GetDeleteApplicationQuery(Guid CandidateId, Guid ApplicationId) : IRequest<GetDeleteApplicationQueryResult>;