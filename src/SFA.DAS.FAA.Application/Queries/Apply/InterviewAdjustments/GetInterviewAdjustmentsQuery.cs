using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.InterviewAdjustments;

public record GetInterviewAdjustmentsQuery : IRequest<GetInterviewAdjustmentsQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
}