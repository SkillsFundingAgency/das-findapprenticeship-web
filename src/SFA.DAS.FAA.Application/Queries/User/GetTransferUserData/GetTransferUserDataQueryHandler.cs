using MediatR;
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
            var applicationsApiResponse = await ApiClient.Get<GetMigrateDataTransferApiResponse>(new GetMigrateDataTransferApiRequest(request.EmailAddress, request.CandidateId));
            
            return new GetTransferUserDataQueryResult
            {
                CandidateFirstName = applicationsApiResponse.FirstName,
                CandidateLastName = applicationsApiResponse.LastName,
                SubmittedApplications = applicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Submitted),
                SavedApplications = applicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Saved),
                StartedApplications = applicationsApiResponse.Applications.Count(fil => fil.Status == LegacyApplicationStatus.Draft),
            };
        }
    }
}
