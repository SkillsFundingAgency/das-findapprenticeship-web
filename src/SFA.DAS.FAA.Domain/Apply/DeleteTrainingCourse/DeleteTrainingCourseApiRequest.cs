
using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse.DeleteTrainingCourseApiRequest;

namespace SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse
{
    public class DeleteTrainingCourseApiRequest(Guid applicationId, Guid trainingCourseId, DeleteTrainingCourseApiRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/trainingcourses/{trainingCourseId}/delete";
        public object Data { get; set; } = body;

        public class DeleteTrainingCourseApiRequestData
        {
            public Guid CandidateId { get; set; }

        }

    }
}
