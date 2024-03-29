﻿using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetTrainingCourse
{
    public class GetTrainingCourseApiRequest(Guid applicationId, Guid candidateId, Guid trainingCourseId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{applicationId}/trainingcourses/{trainingCourseId}?candidateId={candidateId}";
    }

    public class GetTrainingCourseApiResponse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
