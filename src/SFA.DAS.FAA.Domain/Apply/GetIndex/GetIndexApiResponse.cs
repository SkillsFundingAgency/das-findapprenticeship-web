﻿namespace SFA.DAS.FAA.Domain.Apply.GetIndex;

public class GetIndexApiResponse
{
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public DateTime ClosingDate { get; set; }
    public bool IsDisabilityConfident { get; set; }

    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }

    public class EducationHistorySection
    {
        public string Qualifications { get; set; }
        public string TrainingCourses { get; set; }
    }

    public class WorkHistorySection
    {
        public string Jobs { get; set; }
        public string VolunteeringAndWorkExperience { get; set; }
    }

    public class ApplicationQuestionsSection
    {
        public string SkillsAndStrengths { get; set; }
        public string WhatInterestsYou { get; set; }
        public string Travel { get; set; }
    }

    public class InterviewAdjustmentsSection
    {
        public string RequestAdjustments { get; set; }
    }
    public class DisabilityConfidenceSection
    {
        public string InterviewUnderDisabilityConfident { get; set; }
    }
}