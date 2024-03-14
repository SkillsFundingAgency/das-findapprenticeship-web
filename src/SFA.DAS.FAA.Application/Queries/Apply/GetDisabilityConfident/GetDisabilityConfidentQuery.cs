using MediatR;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident
{
    public class GetDisabilityConfidentQuery : IRequest<GetDisabilityConfidentQueryResult>
    {
        public Guid ApplicationId { get; init; }
        public Guid CandidateId { get; init; }
    }

    public class GetDisabilityConfidentQueryResult
    {
        public string EmployerName { get; private init; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public bool? IsSectionCompleted { get; set; }

        public static implicit operator GetDisabilityConfidentQueryResult(GetDisabilityConfidentApiResponse source)
        {
            return new GetDisabilityConfidentQueryResult
            {
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme,
                EmployerName = source.EmployerName,
                IsSectionCompleted = source.IsSectionCompleted
            };
        }
    }

    public class GetDisabilityConfidentQueryHandler(IApiClient apiClient) : IRequestHandler<GetDisabilityConfidentQuery, GetDisabilityConfidentQueryResult>
    {
        public async Task<GetDisabilityConfidentQueryResult> Handle(GetDisabilityConfidentQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetDisabilityConfidentApiResponse>(
                new GetDisabilityConfidentApiRequest(request.ApplicationId, request.CandidateId));

            return response;
        }
    }
}
