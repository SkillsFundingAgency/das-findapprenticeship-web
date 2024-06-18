using MediatR;

namespace SFA.DAS.FAA.Application.Queries.User.GetTransferUserData
{
    public class GetTransferUserDataQuery : IRequest<GetTransferUserDataQueryResult>
    {
        public required Guid CandidateId { get; set; }
        public required string EmailAddress { get; set; }
    }
}
