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

        public static implicit operator GetDeleteJobQueryResult(GetDeleteJobApiResponse source)
        {
            return new GetDeleteJobQueryResult
            {
                Id = source.Id,
                Employer = source.Employer,
                JobTitle = source.JobTitle,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                ApplicationId = source.ApplicationId,
                Description = source.Description
            };
        }
    }

    public class GetDeleteJobQueryHandler(IApiClient ApiClient) : IRequestHandler<GetDeleteJobQuery, GetDeleteJobQueryResult>
    {
        public async Task<GetDeleteJobQueryResult> Handle(GetDeleteJobQuery request, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetDeleteJobApiResponse>(
                new GetDeleteJobApiRequest(request.ApplicationId, request.CandidateId, request.JobId));

            return response;
        }
    }
}
