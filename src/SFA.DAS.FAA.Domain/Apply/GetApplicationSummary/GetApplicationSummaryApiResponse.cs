﻿using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;

public record GetApplicationSummaryApiResponse
{
    public bool IsDisabilityConfident { get; set; }
    public bool IsApplicationComplete { get; set; }
    public CandidateDetailsSection Candidate { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? VacancyTitle { get; set; }
    public string? EmployerName { get; set; }
    public AboutYouSection AboutYou { get; set; }
    public EducationHistorySection EducationHistory { get; set; }
    public WorkHistorySection WorkHistory { get; set; }
    public ApplicationQuestionsSection ApplicationQuestions { get; set; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; set; }
    public DisabilityConfidenceSection DisabilityConfidence { get; set; }
    public WhatIsYourInterestSection WhatIsYourInterest { get; set; }
    public EmploymentLocationSection? EmploymentLocation { get; set; }


    public record CandidateDetailsSection
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressDetailsSection? Address { get; set; }
    }

    public record AddressDetailsSection
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string Postcode { get; set; } = null!;
    }

    public record EducationHistorySection
    {
        public SectionStatus QualificationsStatus { get; set; }
        public SectionStatus TrainingCoursesStatus { get; set; }
        public List<TrainingCourse> TrainingCourses { get; set; } = [];
        public List<Qualification> Qualifications { get; set; }
        public List<QualificationReference> QualificationTypes { get; set; }

        public record TrainingCourse
        {
            public Guid Id { get; set; }
            public string CourseName { get; set; }
            public int YearAchieved { get; set; }
        }
        public record Qualification
        {
            public Guid QualificationReferenceId { get; set; }
            public Guid Id { get; set; }
            public string? Subject { get; set; }
            public string? Grade { get; set; }
            public string? AdditionalInformation { get; set; }
            public bool? IsPredicted { get; set; }
            public short? QualificationOrder { get; set; }
            public QualificationReference QualificationReference { get; set; }
        }

        public record QualificationReference
        {
            public Guid Id { get; set; }
            public string? Name { get; set; }
        }
    }

    public record EmploymentLocationSection : LocationDto
    {
        public SectionStatus EmploymentLocationStatus { get; set; }
    }

    public record WorkHistorySection
    {
        public SectionStatus JobsStatus { get; set; }
        public SectionStatus VolunteeringAndWorkExperienceStatus { get; set; }
        public List<Job> Jobs { get; set; } = [];
        public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = [];

        public record Job
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }
        }

        public record VolunteeringAndWorkExperience
        {
            public Guid Id { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Description { get; set; }
        }
    }

    public record ApplicationQuestionsSection
    {
        public SectionStatus SkillsAndStrengthsStatus { get; set; }
        public SectionStatus WhatInterestsYouStatus { get; set; }
        public Question? AdditionalQuestion1 { get; set; }
        public Question? AdditionalQuestion2 { get; set; }

        public record Question
        {
            public Guid Id { get; set; }
            public SectionStatus Status { get; set; }
            public string QuestionLabel { get; set; }
            public string Answer { get; set; }
        }
    }

    public record InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustmentsStatus { get; set; }
        public string InterviewAdjustmentsDescription { get; set; }
    }

    public record DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfidentStatus { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
    }

    public record WhatIsYourInterestSection
    {
        public string WhatIsYourInterest { get; set; }
    }

    public record AboutYouSection
    {
        public string SkillsAndStrengths { get; set; }
        public string Improvements { get; set; }
        public string HobbiesAndInterests { get; set; }
        public string Support { get; set; }
    }
}