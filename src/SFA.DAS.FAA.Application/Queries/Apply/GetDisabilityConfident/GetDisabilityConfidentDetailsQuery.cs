using MediatR;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDisabilityConfident
{
    public record GetDisabilityConfidentDetailsQuery : IRequest<GetDisabilityConfidentDetailsQueryResult>
    {
        public Guid ApplicationId { get; init; }
        public Guid CandidateId { get; init; }
    }

    public record GetDisabilityConfidentDetailsQueryResult
    {
        public bool? IsSectionCompleted { get; private init; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; private init; }

        public static implicit operator GetDisabilityConfidentDetailsQueryResult(GetDisabilityConfidentDetailsApiResponse source)
        {
            return new GetDisabilityConfidentDetailsQueryResult
            {
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = source.IsSectionCompleted,
            };
        }
    }

    public class GetDisabilityConfidentDetailsQueryHandler(IApiClient apiClient) : IRequestHandler<GetDisabilityConfidentDetailsQuery, GetDisabilityConfidentDetailsQueryResult>
    {
        public async Task<GetDisabilityConfidentDetailsQueryResult> Handle(GetDisabilityConfidentDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetDisabilityConfidentDetailsApiResponse>(
                new GetDisabilityConfidentDetailsApiRequest(request.ApplicationId, request.CandidateId));

            return response;
        }
    }
}
