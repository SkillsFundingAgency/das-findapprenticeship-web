using MediatR;
using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex;

public class GetIndexQueryHandler(IApiClient apiClient) : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
{
    public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
    {
        var applicationsTask = apiClient.Get<GetApplicationsApiResponse>(
            new GetApplicationsApiRequest(request.CandidateId, request.Status));

        var candidateTask = apiClient.Get<GetInformApiResponse>(new GetInformApiRequest(request.CandidateId));

        await Task.WhenAll(applicationsTask, candidateTask);

        return new GetIndexQueryResult
        {
            Applications = applicationsTask.Result.Applications.Select(x => new GetIndexQueryResult.Application
            {
                Id = x.Id,
                Title = x.Title,
                VacancyReference = x.VacancyReference,
                EmployerName = x.EmployerName,
                CreatedDate = x.CreatedDate,
                ClosingDate = x.ClosingDate,
                SubmittedDate = x.SubmittedDate,
                WithdrawnDate = x.WithdrawnDate,
                ResponseDate = x.ResponseDate,
                Status = x.Status,
                ResponseNotes = x.ResponseNotes
            }).ToList(),
            ShowAccountRecoveryBanner = candidateTask.Result.ShowAccountRecoveryBanner
        };
    }
}