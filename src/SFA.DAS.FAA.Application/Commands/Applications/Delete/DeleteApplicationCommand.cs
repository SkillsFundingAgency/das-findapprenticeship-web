using MediatR;

namespace SFA.DAS.FAA.Application.Commands.Applications.Delete;

public record DeleteApplicationCommand(Guid CandidateId, Guid ApplicationId) : IRequest<DeleteApplicationCommandResult>;