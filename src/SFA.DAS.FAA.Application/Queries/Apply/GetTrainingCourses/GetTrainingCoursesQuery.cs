using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
public record GetTrainingCoursesQuery : IRequest<GetTrainingCoursesQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
}
