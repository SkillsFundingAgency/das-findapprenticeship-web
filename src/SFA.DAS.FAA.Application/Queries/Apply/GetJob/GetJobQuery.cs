using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetJob
{
    public class GetJobQuery : IRequest<GetJobQueryResult>
    {
        public Guid CandidateId { get; init; }
        public Guid ApplicationId { get; init; }
        public Guid JobId { get; set; }
    }

    public class GetJobQueryResult
    {
        public Guid Id { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }

        public static implicit operator GetJobQueryResult(GetJobApiResponse source)
        {
            return new GetJobQueryResult
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

    public class GetJobQueryHandler(IApiClient ApiClient) : IRequestHandler<GetJobQuery, GetJobQueryResult>
    {
        public async Task<GetJobQueryResult> Handle(GetJobQuery request, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetJobApiResponse>(
                new GetJobApiRequest(request.ApplicationId, request.CandidateId, request.JobId));

            return response;
        }
    }
}
