using MediatR;
using SFA.DAS.FAA.Domain.Applications.DeleteApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Infrastructure.Api;

namespace SFA.DAS.FAA.Application.Commands.Applications.Delete;

public class DeleteApplicationCommandHandler(IApiClient apiClient): IRequestHandler<DeleteApplicationCommand, DeleteApplicationCommandResult>
{
    public async Task<DeleteApplicationCommandResult> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCode<NullResponse>(
            new PostDeleteApplicationApiRequest(request.ApplicationId, request.CandidateId)
        );
        
        return new DeleteApplicationCommandResult { Success = false };
    }
}