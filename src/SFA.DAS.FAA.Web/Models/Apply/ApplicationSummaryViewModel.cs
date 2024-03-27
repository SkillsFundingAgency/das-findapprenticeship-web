using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class ApplicationSummaryViewModel
{
    public Guid ApplicationId { get; set; }

    [BindProperty]
    public bool IsConsentProvided { get; set; }
    public CandidateDetailsSection Candidate { get; init; } = new();

    public static implicit operator ApplicationSummaryViewModel(GetApplicationSummaryQueryResult source)
    {
        return new ApplicationSummaryViewModel
        {
            Candidate = source.Candidate,
            ApplicationQuestions = source.ApplicationQuestions,
            DisabilityConfidence = source.DisabilityConfidence,
            EducationHistory = source.EducationHistory,
            InterviewAdjustments = source.InterviewAdjustments,
            WorkHistory = source.WorkHistory,
            IsDisabilityConfident = source.IsDisabilityConfident,
        };
    }

    public class CandidateDetailsSection
    {
        public Guid Id { get; set; }
        public string? GovUkIdentifier { get; set; }
        public string? Email { get; init; }
        public string FullName => $"{FirstName} {LastName}";
        private string? FirstName { get; init; }
        public string? MiddleName { get; set; }
        private string? LastName { get; init; }
        public string? PhoneNumber { get; init; }
        private DateTime? DateOfBirth { get; init; }
        public string BirthDate => DateOfBirth.HasValue ? DateOfBirth.Value.ToString("dd MMM yyyy") : string.Empty;
        public AddressDetailsSection? Address { get; init; } = new();

        public static implicit operator CandidateDetailsSection(GetApplicationSummaryQueryResult.CandidateDetailsSection source)
        {
            return new CandidateDetailsSection
            {
                Id = source.Id,
                GovUkIdentifier = source.GovUkIdentifier,
                Email = source.Email,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                PhoneNumber = source.PhoneNumber,
                DateOfBirth = source.DateOfBirth,
                Address = source.Address
            };
        }
    }

    public class AddressDetailsSection
    {
        public string? AddressLine1 { get; init; }
        public string? AddressLine2 { get; init; }
        public string? Town { get; init; }
        public string? County { get; init; }
        public string? Postcode { get; init; }

        public static implicit operator AddressDetailsSection?(GetApplicationSummaryQueryResult.AddressDetailsSection? source)
        {
            if (source is null) return null;

            return new AddressDetailsSection
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                Town = source.Town,
                County = source.County,
                Postcode = source.Postcode,
            };
        }
    }

    public bool IsDisabilityConfident { get; init; }

    public bool IsApplicationComplete => EducationHistory is {TrainingCoursesStatus: SectionStatus.Completed, QualificationsStatus: SectionStatus.Completed} &&
                                         WorkHistory is {VolunteeringAndWorkExperienceStatus: SectionStatus.Completed, JobsStatus: SectionStatus.Completed} &&
                                         (ApplicationQuestions.AdditionalQuestion1Id is not null || ApplicationQuestions.AdditionalQuestion1Status == SectionStatus.Completed) &&
                                         (ApplicationQuestions.AdditionalQuestion2Id is not null || ApplicationQuestions.AdditionalQuestion2Status == SectionStatus.Completed) &&
                                         InterviewAdjustments.RequestAdjustmentsStatus == SectionStatus.Completed &&
                                         (!IsDisabilityConfident || DisabilityConfidence.InterviewUnderDisabilityConfidentStatus == SectionStatus.Completed);

    public EducationHistorySection EducationHistory { get; init; } = new();
    public WorkHistorySection WorkHistory { get; init; } = new();
    public ApplicationQuestionsSection ApplicationQuestions { get; init; } = new();
    public InterviewAdjustmentsSection InterviewAdjustments { get; init; } = new();
    public DisabilityConfidenceSection DisabilityConfidence { get; init; } = new();

    public class EducationHistorySection
    {
        public SectionStatus QualificationsStatus { get; private init; }
        public SectionStatus TrainingCoursesStatus { get; private init; }

        public static implicit operator EducationHistorySection(GetApplicationSummaryQueryResult.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                QualificationsStatus = source.QualificationsStatus,
                TrainingCoursesStatus = source.TrainingCoursesStatus
            };
        }
    }

    public class WorkHistorySection
    {
        public SectionStatus JobsStatus { get; private init; }
        public SectionStatus VolunteeringAndWorkExperienceStatus { get; private init; }

        public static implicit operator WorkHistorySection(GetApplicationSummaryQueryResult.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                JobsStatus = source.JobsStatus,
                VolunteeringAndWorkExperienceStatus = source.VolunteeringAndWorkExperienceStatus
            };
        }
    }

    public class ApplicationQuestionsSection
    {
        public SectionStatus SkillsAndStrengthsStatus { get; set; }
        public SectionStatus WhatInterestsYouStatus { get; set; }
        public SectionStatus AdditionalQuestion1Status { get; private init; }
        public SectionStatus AdditionalQuestion2Status { get; private init; }
        public Guid? AdditionalQuestion1Id { get; private init; }
        public Guid? AdditionalQuestion2Id { get; private init; }

        public static implicit operator ApplicationQuestionsSection(GetApplicationSummaryQueryResult.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                SkillsAndStrengthsStatus = source.SkillsAndStrengthsStatus,
                WhatInterestsYouStatus = source.WhatInterestsYouStatus,
                AdditionalQuestion1Status = source.AdditionalQuestion1Status,
                AdditionalQuestion2Status = source.AdditionalQuestion2Status,
                AdditionalQuestion1Id = source.AdditionalQuestion1Id,
                AdditionalQuestion2Id = source.AdditionalQuestion2Id,
            };
        }
    }

    public class InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustmentsStatus { get; private init; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationSummaryQueryResult.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustmentsStatus = source.RequestAdjustmentsStatus
            };
        }
    }
    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfidentStatus { get; private init; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationSummaryQueryResult.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfidentStatus = source.InterviewUnderDisabilityConfidentStatus
            };
        }
    }
}