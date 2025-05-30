﻿using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse
{
    public class GetDeleteTrainingCourseApiRequest(Guid applicationId, Guid candidateId, Guid trainingCourseId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}/trainingcourses/{trainingCourseId}/delete?candidateId={candidateId}";
    }

    public class GetDeleteTrainingCourseApiResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
