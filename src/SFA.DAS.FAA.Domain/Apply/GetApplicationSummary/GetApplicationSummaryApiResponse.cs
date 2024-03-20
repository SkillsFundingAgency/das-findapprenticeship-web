using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;

public class GetApplicationSummaryApiResponse
{
    public bool IsDisabilityConfident { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }

    public CandidateDetailsSection Candidate { get; set; }

    public class CandidateDetailsSection
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public AddressDetailsSection? Address { get; set; }
    }

    public class AddressDetailsSection
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string Postcode { get; set; } = null!;
    }
    

    public class EducationHistorySection
    {
        public SectionStatus QualificationsStatus { get; set; }
        public SectionStatus TrainingCoursesStatus { get; set; }
    }

    public class WorkHistorySection
    {
        public SectionStatus JobsStatus { get; set; }
        public SectionStatus VolunteeringAndWorkExperienceStatus { get; set; }
    }

    public class ApplicationQuestionsSection
    {
        public SectionStatus SkillsAndStrengthsStatus { get; set; }
        public SectionStatus WhatInterestsYouStatus { get; set; }
        public SectionStatus AdditionalQuestion1Status { get; set; }
        public SectionStatus AdditionalQuestion2Status { get; set; }
        public Guid? AdditionalQuestion1Id { get; set; }
        public Guid? AdditionalQuestion2Id { get; set; }
        
    }

    public class InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustmentsStatus { get; set; }
    }

    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfidentStatus { get; set; }
    }
}