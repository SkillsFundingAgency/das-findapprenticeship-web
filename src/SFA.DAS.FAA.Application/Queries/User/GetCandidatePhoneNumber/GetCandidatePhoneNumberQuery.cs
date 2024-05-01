using MediatR;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.User;

namespace CreateAccount.GetCandidatePhoneNumber
{
    public class GetCandidatePhoneNumberQuery : IRequest<GetCandidatePhoneNumberQueryResult>
    {
        public Guid CandidateId { get; set; }
    }

    public class GetCandidatePhoneNumberQueryResult
    {
        public static implicit operator GetCandidatePhoneNumberQueryResult(GetCandidatePhoneNumberApiResponse source)
        {
            return new GetCandidatePhoneNumberQueryResult
            {
                IsAddressFromLookup = source.IsAddressFromLookup,
                PhoneNumber = source.PhoneNumber,
                Postcode = source.Postcode
            };
        }

        public bool IsAddressFromLookup { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Postcode { get; set; }
    }

    public class GetCandidatePhoneNumberQueryHandler(IApiClient apiClient) : IRequestHandler<GetCandidatePhoneNumberQuery, GetCandidatePhoneNumberQueryResult>
    {
        public async Task<GetCandidatePhoneNumberQueryResult> Handle(GetCandidatePhoneNumberQuery request, CancellationToken cancellationToken)
        {
            return await apiClient.Get<GetCandidatePhoneNumberApiResponse>(new GetCandidatePhoneNumberApiRequest(request.CandidateId));
        }
    }

}
