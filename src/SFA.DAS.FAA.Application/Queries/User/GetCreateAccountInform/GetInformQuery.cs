using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Application.Queries.User.GetCreateAccountInform
{
    public class GetInformQuery : IRequest<GetInformQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetInformQueryResult
    {
        public bool IsAccountCreated { get; set; }

        public static implicit operator GetInformQueryResult(GetInformApiResponse source)
        {
            return new GetInformQueryResult
            {
                IsAccountCreated = source.IsAccountCreated
            };
        }
    }

    public class GetInformQueryHandler(IApiClient apiClient) : IRequestHandler<GetInformQuery, GetInformQueryResult>
    {
        public async Task<GetInformQueryResult> Handle(GetInformQuery request, CancellationToken cancellationToken)
        {
            return await apiClient.Get<GetInformApiResponse>(new GetInformApiRequest(request.CandidateId));
        }
    }
}
