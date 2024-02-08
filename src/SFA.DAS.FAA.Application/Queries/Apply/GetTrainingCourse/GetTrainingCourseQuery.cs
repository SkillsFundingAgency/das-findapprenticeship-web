using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;

public class GetTrainingCourseQuery : IRequest<GetTrainingCourseQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
    public Guid TrainingCourseId { get; set; }
}
