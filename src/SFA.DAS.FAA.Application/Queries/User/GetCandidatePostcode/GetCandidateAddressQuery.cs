using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;
public class GetCandidateAddressQuery : IRequest<GetCandidateAddressQueryResult>
{
    public Guid CandidateId { get; set; }
}
