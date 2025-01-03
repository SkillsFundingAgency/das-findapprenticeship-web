using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;

public class GetDeleteTrainingCourseQuery : IRequest<GetDeleteTrainingCourseQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
    public Guid TrainingCourseId { get; set; }
}
