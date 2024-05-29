using SFA.DAS.FAA.Domain.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetIndex;

public class GetIndexQueryResult
{
    public string VacancyReference { get; set; }
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public DateTime ClosingDate { get; set; }
    public bool IsMigrated { get; set; }
    public bool IsDisabilityConfident { get; set; }
    public bool IsApplicationComplete { get; set; }
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

        public static implicit operator EducationHistorySection(GetIndexApiResponse.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                Qualifications = source.Qualifications,
                TrainingCourses = source.TrainingCourses
            };
        }
    }

    public class WorkHistorySection
    {
        public SectionStatus Jobs { get; set; }
        public SectionStatus VolunteeringAndWorkExperience { get; set; }

        public static implicit operator WorkHistorySection(GetIndexApiResponse.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                Jobs = source.Jobs,
                VolunteeringAndWorkExperience = source.VolunteeringAndWorkExperience
            };
        }
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

        public static implicit operator ApplicationQuestionsSection(GetIndexApiResponse.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                SkillsAndStrengths = source.SkillsAndStrengths,
                WhatInterestsYou = source.WhatInterestsYou,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalQuestion1Label = source.AdditionalQuestion1Label,
                AdditionalQuestion2Label = source.AdditionalQuestion2Label,
                AdditionalQuestion1Id = source.AdditionalQuestion1Id,
                AdditionalQuestion2Id = source.AdditionalQuestion2Id
            };
        }
    }

    public class InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustments { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetIndexApiResponse.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustments = source.RequestAdjustments
            };
        }
    }
    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfident { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetIndexApiResponse.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfident = source.InterviewUnderDisabilityConfident
            };
        }
    }

    public class PreviousApplicationDetails
    {
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public DateTime SubmissionDate { get; set; }

        public static implicit operator PreviousApplicationDetails?(GetIndexApiResponse.PreviousApplicationDetails? source)
        {
            if (source == null) return null;

            return new PreviousApplicationDetails
            {
                EmployerName = source.EmployerName,
                SubmissionDate = source.SubmissionDate,
                VacancyTitle = source.VacancyTitle
            };
        }
    }

    public static implicit operator GetIndexQueryResult(GetIndexApiResponse source)
    {
        return new GetIndexQueryResult
        {
            VacancyReference = source.VacancyReference,
            VacancyTitle = source.VacancyTitle,
            EmployerName = source.EmployerName,
            ClosingDate = source.ClosingDate,
            IsMigrated = source.IsMigrated,
            IsDisabilityConfident = source.IsDisabilityConfident,
            IsApplicationComplete = source.IsApplicationComplete,
            EducationHistory = source.EducationHistory,
            WorkHistory = source.WorkHistory,
            ApplicationQuestions = source.ApplicationQuestions,
            InterviewAdjustments = source.InterviewAdjustments,
            DisabilityConfidence = source.DisabilityConfidence,
            PreviousApplication = source.PreviousApplication
        };
    }
}