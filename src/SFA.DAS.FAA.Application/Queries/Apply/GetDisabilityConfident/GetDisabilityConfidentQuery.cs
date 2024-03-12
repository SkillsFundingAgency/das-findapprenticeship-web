using MediatR;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident
{
    public class GetDisabilityConfidentQuery : IRequest<GetDisabilityConfidentQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }

    public class GetDisabilityConfidentQueryResult
    {
        public string EmployerName { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }

        public static implicit operator GetDisabilityConfidentQueryResult(GetDisabilityConfidentApiResponse source)
        {
            return new GetDisabilityConfidentQueryResult
            {
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme,
                EmployerName = source.EmployerName
            };
        }
    }

    public class GetDisabilityConfidentQueryHandler(IApiClient ApiClient) : IRequestHandler<GetDisabilityConfidentQuery, GetDisabilityConfidentQueryResult>
    {
        public async Task<GetDisabilityConfidentQueryResult> Handle(GetDisabilityConfidentQuery request, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetDisabilityConfidentApiResponse>(
                new GetDisabilityConfidentApiRequest(request.ApplicationId, request.CandidateId));

            return response;
        }
    }
}
