using MediatR;
using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public Guid CandidateId { get; set; }
        public ApplicationStatus Status { get; set; }
    }

    public class GetIndexQueryResult
    {
        public List<Application> Applications { get; set; }

        public class Application
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string EmployerName { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime ClosingDate { get; set; }
        }

        public static implicit operator GetIndexQueryResult(GetApplicationsApiResponse source)
        {
            return new GetIndexQueryResult
            {
                Applications = source.Applications.Select(x => new Application
                    {
                        Id = x.Id,
                        Title = x.Title,
                        EmployerName = x.EmployerName,
                        CreatedDate = x.CreatedDate,
                        ClosingDate = x.ClosingDate,
                    }).ToList()
            };
        }
    }

    public class GetIndexQueryHandler(IApiClient apiClient) : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
    {
        public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetApplicationsApiResponse>(
                new GetApplicationsApiRequest(request.CandidateId, request.Status));

            return response;
        }
    }
}
