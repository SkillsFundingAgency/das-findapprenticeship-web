using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.AddTrainingCourse;
public class PostTrainingCourseApiRequest (Guid applicationId, PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData body)
        : IPostApiRequest
{
    public string PostUrl => $"applications/{applicationId}/training-courses";
    public object Data { get; set; } = body;

    public class PostTrainingCourseApiRequestData
    {
        public Guid CandidateId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
