using MediatR;
using SFA.DAS.FAA.Domain.Apply.WhatInterestsYou;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou;

public class GetWhatInterestsYouQueryHandler(IApiClient ApiClient) : IRequestHandler<GetWhatInterestsYouQuery, GetWhatInterestsYouQueryResult>
{
    public async Task<GetWhatInterestsYouQueryResult> Handle(GetWhatInterestsYouQuery request, CancellationToken cancellationToken)
    {
        var response = await ApiClient.Get<GetWhatInterestsYouApiResponse>(
            new GetWhatInterestsYouApiRequest(request.ApplicationId, request.CandidateId));

        return response;
    }
}