﻿using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetTrainingCourses;
public class GetTrainingCoursesApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{ApplicationId}/trainingcourses?candidateId={CandidateId}";
}
