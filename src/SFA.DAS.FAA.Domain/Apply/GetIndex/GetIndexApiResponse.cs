using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Apply.GetIndex;

public class GetIndexApiResponse
{
    public string VacancyReference { get; set; }
    public string VacancyTitle { get; set; }
    public ApprenticeshipTypes ApprenticeshipType { get; set; }
    public string EmployerName { get; set; }
    public DateTime ClosingDate { get; set; }
    public bool IsMigrated { get; set; }
    public bool IsDisabilityConfident { get; set; }
    public bool IsApplicationComplete { get; set; }

    public AvailableWhere? EmployerLocationOption { get; set; }
    public Address? Address { get; set; }
    public List<Address>? OtherAddresses { get; set; }

    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }
    public PreviousApplicationDetails? PreviousApplication { get; set; }

    public class EducationHistorySection
    {
        public SectionStatus Qualifications { get; set; }
        public SectionStatus TrainingCourses { get; set; }
    }

    public class WorkHistorySection
    {
        public SectionStatus Jobs { get; set; }
        public SectionStatus VolunteeringAndWorkExperience { get; set; }
    }

    public class ApplicationQuestionsSection
    {
        public SectionStatus SkillsAndStrengths { get; set; }
        public SectionStatus WhatInterestsYou { get; set; }
        public SectionStatus AdditionalQuestion1 { get; set; }
        public SectionStatus AdditionalQuestion2 { get; set; }
        public string? AdditionalQuestion1Label { get; set; }
        public string? AdditionalQuestion2Label { get; set; }
        public Guid? AdditionalQuestion1Id { get; set; }
        public Guid? AdditionalQuestion2Id { get; set; }
    }

    public class InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustments { get; set; }
    }

    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfident { get; set; }
    }

    public class PreviousApplicationDetails
    {
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}