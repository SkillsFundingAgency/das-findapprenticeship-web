using MediatR;
using SFA.DAS.FAA.Domain.Applications.WithdrawApplication;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.Withdraw;

public class GetWithdrawApplicationQueryHandler(IApiClient apiClient) : IRequestHandler<GetWithdrawApplicationQuery, GetWithdrawApplicationQueryResult>
{
    public async Task<GetWithdrawApplicationQueryResult> Handle(GetWithdrawApplicationQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetWithdrawApplicationApiResponse>(
            new GetWithdrawApplicationApiRequest(request.ApplicationId, request.CandidateId)
        );

        if (response == null)
            throw new ResourceNotFoundException();

        var addresses = new List<Address> { response.Address };
        if (response.OtherAddresses is { Count: > 0 }) addresses.AddRange(response.OtherAddresses);

        return new GetWithdrawApplicationQueryResult
        {
            ApplicationId = response.ApplicationId,
            ClosingDate = response.ClosingDate,
            ClosedDate = response.ClosedDate,
            EmployerName = response.EmployerName,
            SubmittedDate = response.SubmittedDate,
            AdvertTitle = response.AdvertTitle,
            WorkLocation = response.Address,
            Addresses = addresses,
            EmploymentLocationInformation = response.EmploymentLocationInformation,
            EmployerLocationOption = response.EmployerLocationOption,
            ApprenticeshipType = response.ApprenticeshipType
        };
    }
}