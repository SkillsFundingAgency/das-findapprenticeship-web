﻿using MediatR;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetTransferUserData
{
    public record GetTransferUserDataQueryHandler(IApiClient ApiClient)
        : IRequestHandler<GetTransferUserDataQuery, GetTransferUserDataQueryResult>
    {
        public async Task<GetTransferUserDataQueryResult> Handle(GetTransferUserDataQuery request,
            CancellationToken cancellationToken)
        {
            var applicationsApiResponseTask = ApiClient.Get<GetMigrateDataTransferApiResponse>(new GetMigrateDataTransferApiRequest(request.EmailAddress));
            var candidateApiResponseTask = ApiClient.Get<GetCandidateNameApiResponse>(new GetCandidateNameApiRequest(request.CandidateId));

            await Task.WhenAll(applicationsApiResponseTask, candidateApiResponseTask);

            var applicationsApiResponse = applicationsApiResponseTask.Result;
            var candidateApiResponse = candidateApiResponseTask.Result;

            return new GetTransferUserDataQueryResult
            {
                CandidateFirstName = candidateApiResponse.FirstName,
                CandidateLastName = candidateApiResponse.LastName,
                SubmittedApplications = applicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Submitted),
                SavedApplications = applicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Saved),
                StartedApplications = applicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Draft),
            };
        }
    }
}