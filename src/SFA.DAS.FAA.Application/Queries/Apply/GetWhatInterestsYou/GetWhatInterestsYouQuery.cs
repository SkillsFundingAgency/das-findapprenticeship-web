using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWhatInterestsYou
{
    public class GetWhatInterestsYouQuery : IRequest<GetWhatInterestsYouQueryResult>
    {
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
