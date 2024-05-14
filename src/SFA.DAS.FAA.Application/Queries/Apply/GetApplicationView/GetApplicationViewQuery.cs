using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView
{
    public class GetApplicationViewQuery : IRequest<GetApplicationViewQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
    }
}
