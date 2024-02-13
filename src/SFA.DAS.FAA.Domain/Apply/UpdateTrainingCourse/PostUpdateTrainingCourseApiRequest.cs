using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpdateTrainingCourse;
public class PostUpdateTrainingCourseApiRequest(Guid applicationId, Guid trainingCourseId, PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData body) : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/trainingcourses/{trainingCourseId}";
    public object Data { get; set; } = body;

    public class PostUpdateTrainingCourseRequestData
    {
        public Guid CandidateId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
