using SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;

public class GetApplicationSummaryQueryResult
{
    public bool IsDisabilityConfident { get; set; }
    public EducationHistorySection EducationHistory { get; init; }
    public WorkHistorySection WorkHistory { get; init; } 
    public ApplicationQuestionsSection ApplicationQuestions { get; init; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; init; }
    public DisabilityConfidenceSection DisabilityConfidence { get; init; }
    public CandidateDetailsSection Candidate { get; init; }


    public static implicit operator GetApplicationSummaryQueryResult(GetApplicationSummaryApiResponse source)
    {
        return new GetApplicationSummaryQueryResult
        {
            Candidate = source.Candidate,
            WorkHistory = source.WorkHistory,
            ApplicationQuestions = source.ApplicationQuestions,
            InterviewAdjustments = source.InterviewAdjustments,
            DisabilityConfidence = source.DisabilityConfidence,
            EducationHistory = source.EducationHistory,
            IsDisabilityConfident = source.IsDisabilityConfident
        };
    }

    public class CandidateDetailsSection
    {
        public Guid Id { get; init; }
        public string GovUkIdentifier { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string? FirstName { get; init; }
        public string? MiddleName { get; init; }
        public string? LastName { get; init; }
        public string? PhoneNumber { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public AddressDetailsSection? Address { get; init; }

        public static implicit operator CandidateDetailsSection(GetApplicationSummaryApiResponse.CandidateDetailsSection source)
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
        public string AddressLine1 { get; init; }
        public string? AddressLine2 { get; init; }
        public string? Town { get; init; }
        public string? County { get; init; }
        public string Postcode { get; init; }

        public static implicit operator AddressDetailsSection?(GetApplicationSummaryApiResponse.AddressDetailsSection? source)
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

    public class EducationHistorySection
    {
        public SectionStatus QualificationsStatus { get; set; }
        public SectionStatus TrainingCoursesStatus { get; set; }

        public static implicit operator EducationHistorySection(GetApplicationSummaryApiResponse.EducationHistorySection source)
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
        public SectionStatus JobsStatus { get; set; }
        public SectionStatus VolunteeringAndWorkExperienceStatus { get; set; }

        public static implicit operator WorkHistorySection(GetApplicationSummaryApiResponse.WorkHistorySection source)
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
        public SectionStatus SkillsAndStrengthsStatus { get; init; }
        public SectionStatus WhatInterestsYouStatus { get; init; }
        public SectionStatus AdditionalQuestion1Status { get; set; }
        public SectionStatus AdditionalQuestion2Status { get; set; }
        public Guid? AdditionalQuestion1Id { get; init; }
        public Guid? AdditionalQuestion2Id { get; init; }

        public static implicit operator ApplicationQuestionsSection(GetApplicationSummaryApiResponse.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                SkillsAndStrengthsStatus = source.SkillsAndStrengthsStatus,
                WhatInterestsYouStatus = source.WhatInterestsYouStatus,
                AdditionalQuestion1Status = source.AdditionalQuestion1Status,
                AdditionalQuestion2Status = source.AdditionalQuestion2Status,
                AdditionalQuestion1Id = source.AdditionalQuestion1Id,
                AdditionalQuestion2Id = source.AdditionalQuestion2Id
            };
        }
    }

    public class InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustmentsStatus { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationSummaryApiResponse.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustmentsStatus = source.RequestAdjustmentsStatus
            };
        }
    }

    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfidentStatus { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationSummaryApiResponse.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfidentStatus = source.InterviewUnderDisabilityConfidentStatus
            };
        }
    }
}