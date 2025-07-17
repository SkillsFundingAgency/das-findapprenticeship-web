using System.Net;
using MediatR;
using SFA.DAS.FAA.Domain.Applications.DeleteApplication;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.Applications.Delete;

public class DeleteApplicationCommandHandler(IApiClient apiClient): IRequestHandler<DeleteApplicationCommand, DeleteApplicationCommandResult>
{
    public async Task<DeleteApplicationCommandResult> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var response = await apiClient.PostWithResponseCodeAsync<PostDeleteApplicationApiResponse>(
            new PostDeleteApplicationApiRequest(request.ApplicationId, request.CandidateId),
            cancellationToken
        );

        return response.StatusCode.IsSuccessCode()
            ? new DeleteApplicationCommandResult
                {
                    Success = true,
                    EmployerName = response.Body.EmployerName,
                    VacancyTitle = response.Body.VacancyTitle,
                }
            : new DeleteApplicationCommandResult { Success = false };
    }
}