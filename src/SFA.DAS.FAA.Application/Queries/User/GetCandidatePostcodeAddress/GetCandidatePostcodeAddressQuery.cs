using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcodeAddress;
public class GetCandidatePostcodeAddressQuery : IRequest<GetCandidatePostcodeAddressQueryResult>
{
    public string Postcode { get; set; }
}
