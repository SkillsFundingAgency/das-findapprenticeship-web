using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetTransferUserData
{
    public class GetTransferUserDataQuery : IRequest<GetTransferUserDataQueryResult>
    {
        public required Guid CandidateId { get; set; }
        public required string EmailAddress { get; set; }
    }
}
