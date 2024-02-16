using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteJob
{
    public class GetDeleteJobQuery : IRequest<GetDeleteJobQueryResult>
    {
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
        public Guid JobId { get; set; }
    }

    public class GetDeleteJobQueryResult
    {
        public Guid Id { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }

    }

    public class GetDeleteJobQueryHandler : IRequestHandler<GetDeleteJobQuery, GetDeleteJobQueryResult>
    {
        private readonly IApiClient _apiClient;

        public GetDeleteJobQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetDeleteJobQueryResult> Handle(GetDeleteJobQuery query, CancellationToken cancellationToken)
        {
            var request = new GetDeleteJobApiRequest(query.ApplicationId, query.CandidateId, query.JobId);

            var response = await _apiClient.Get<GetDeleteJobApiResponse>(request);

            return new GetDeleteJobQueryResult
            {
                Id = response.Id,
                Employer = response.Employer,
                JobTitle = response.JobTitle,
                StartDate = response.StartDate,
                EndDate = response.EndDate,
                ApplicationId = response.ApplicationId,
                Description = response.Description,
            };
        }
    }
}
