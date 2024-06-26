﻿using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetCandidateDateOfBirth;
public class GetCandidateDateOfBirthQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetCandidateDateOfBirthQuery, GetCandidateDateOfBirthQueryResult>
{
    public async Task<GetCandidateDateOfBirthQueryResult> Handle(GetCandidateDateOfBirthQuery request, CancellationToken cancellationToken)
    {
        return await apiClient.Get<GetCandidateDateOfBirthApiResponse>(new GetCandidateDateOfBirthApiRequest(request.CandidateId));
    }
}
