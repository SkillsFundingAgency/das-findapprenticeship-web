using System.Net;
using MediatR;
using SFA.DAS.FAA.Domain.Applications.DeleteApplication;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.Delete;

public class GetDeleteApplicationQueryHandler(IApiClient apiClient) : IRequestHandler<GetDeleteApplicationQuery, GetDeleteApplicationQueryResult>
{
    public async Task<GetDeleteApplicationQueryResult> Handle(GetDeleteApplicationQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.GetWithResponseCode<GetConfirmDeleteApplicationApiResponse?>(
            new GetConfirmDeleteApplicationApiRequest(request.ApplicationId, request.CandidateId)
        );

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            return GetDeleteApplicationQueryResult.None;
        }

        var result = response.Body;
        List<Address> addresses = [result.Address!];
        if (result.OtherAddresses is { Count: > 0 })
        {
            addresses.AddRange(result.OtherAddresses);
        }

        return new GetDeleteApplicationQueryResult
        {
            Addresses = addresses,
            ApplicationId = result.ApplicationId,
            ApplicationStartDate = result.ApplicationStartDate!.Value,
            ApprenticeshipType = result.ApprenticeshipType!.Value,
            EmployerLocationOption = result.EmployerLocationOption,
            EmployerName = result.EmployerName,
            VacancyClosedDate = result.VacancyClosedDate,
            VacancyClosingDate = result.VacancyClosingDate!.Value,
            VacancyTitle = result.VacancyTitle,
        };
    }
}