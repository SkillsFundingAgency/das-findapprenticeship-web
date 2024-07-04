using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetCandidatePostcode;

public class GetCandidatePostcodeQuery : IRequest<GetCandidatePostcodeQueryResult>
{
    public Guid CandidateId { get; set; }
}