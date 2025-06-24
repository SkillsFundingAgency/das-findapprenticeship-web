using SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;

public class GetApplicationSummaryQueryResult
{
    public bool IsDisabilityConfident { get; set; }
    public bool IsApplicationComplete { get; set; }
    public CandidateDetailsSection Candidate { get; init; }
    public DateTime ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? VacancyTitle { get; set; }
    public string? EmployerName { get; set; }
    public AboutYouSection AboutYou { get; init; }
    public EducationHistorySection EducationHistory { get; init; }
    public WorkHistorySection WorkHistory { get; init; } 
    public ApplicationQuestionsSection ApplicationQuestions { get; init; }
    public InterviewAdjustmentsSection InterviewAdjustments { get; init; }
    public DisabilityConfidenceSection DisabilityConfidence { get; init; }
    public WhatIsYourInterestSection WhatIsYourInterest { get; init; }


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
            IsDisabilityConfident = source.IsDisabilityConfident,
            AboutYou = source.AboutYou,
            WhatIsYourInterest = source.WhatIsYourInterest,
            IsApplicationComplete = source.IsApplicationComplete,
            ClosedDate = source.ClosedDate,
            ClosingDate = source.ClosingDate,
            EmployerName = source.EmployerName,
            VacancyTitle = source.VacancyTitle,
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
        public List<TrainingCourse?> TrainingCourses { get; set; } = [];
        public List<Qualification?> Qualifications { get; set; } = [];
        public List<QualificationReference?> QualificationTypes { get; set; } = [];

        public static implicit operator EducationHistorySection(GetApplicationSummaryApiResponse.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                QualificationsStatus = source.QualificationsStatus,
                TrainingCoursesStatus = source.TrainingCoursesStatus,
                TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList(),
                Qualifications = source.Qualifications.Select(x => (Qualification)x).OrderBy(ord => ord?.QualificationOrder).ToList(),
                QualificationTypes = source.QualificationTypes.Select(x => (QualificationReference)x).ToList()
            };
        }

        public record TrainingCourse
        {
            public Guid Id { get; set; }
            public string? CourseName { get; set; }
            public int YearAchieved { get; set; }

            public static implicit operator TrainingCourse?(GetApplicationSummaryApiResponse.EducationHistorySection.TrainingCourse? source)
            {
                if (source is null) return null;

                return new TrainingCourse
                {
                    Id = source.Id,
                    CourseName = source.CourseName,
                    YearAchieved = source.YearAchieved
                };
            }
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
            public QualificationReference? QualificationReference { get; set; }

            public static implicit operator Qualification?(GetApplicationSummaryApiResponse.EducationHistorySection.Qualification? source)
            {
                if (source is null) return null;

                return new Qualification
                {
                    Id = source.Id,
                    Subject = source.Subject,
                    Grade = source.Grade,
                    AdditionalInformation = source.AdditionalInformation,
                    IsPredicted = source.IsPredicted,
                    QualificationOrder = source.QualificationOrder,
                    QualificationReferenceId = source.QualificationReferenceId,
                    QualificationReference = source.QualificationReference
                };
            }
        }

        public record QualificationReference
        {
            public Guid Id { get; set; }
            public string? Name { get; set; }

            public static implicit operator QualificationReference?(GetApplicationSummaryApiResponse.EducationHistorySection.QualificationReference? source)
            {
                if (source is null) return null;

                return new QualificationReference
                {
                    Id = source.Id,
                    Name = source.Name
                };
            }
        }
    }

    public class WorkHistorySection
    {
        public SectionStatus JobsStatus { get; set; }
        public SectionStatus VolunteeringAndWorkExperienceStatus { get; set; }
        public List<Job?> Jobs { get; set; } = [];
        public List<VolunteeringAndWorkExperience?> VolunteeringAndWorkExperiences { get; set; } = [];

        public static implicit operator WorkHistorySection(GetApplicationSummaryApiResponse.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                JobsStatus = source.JobsStatus,
                VolunteeringAndWorkExperienceStatus = source.VolunteeringAndWorkExperienceStatus,
                Jobs = source.Jobs.Select(x => (Job)x).ToList(),
                VolunteeringAndWorkExperiences = source.VolunteeringAndWorkExperiences.Select(x => (VolunteeringAndWorkExperience)x).ToList(),
            };
        }

        public record Job
        {
            public Guid Id { get; set; }
            public string? Employer { get; set; }
            public string? JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string? Description { get; set; }

            public static implicit operator Job?(GetApplicationSummaryApiResponse.WorkHistorySection.Job? source)
            {
                if (source is null) return null;

                return new Job
                {
                    Id = source.Id,
                    Employer = source.Employer,
                    JobTitle = source.JobTitle,
                    StartDate = source.StartDate,
                    EndDate = source.EndDate,
                    Description = source.Description
                };
            }
        }

        public record VolunteeringAndWorkExperience
        {
            public Guid Id { get; set; }
            public string? Employer { get; set; }
            public string? JobTitle { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string? Description { get; set; }

            public static implicit operator VolunteeringAndWorkExperience?(GetApplicationSummaryApiResponse.WorkHistorySection.VolunteeringAndWorkExperience? source)
            {
                if (source is null) return null;

                return new VolunteeringAndWorkExperience
                {
                    Id = source.Id,
                    Employer = source.Employer,
                    JobTitle = source.JobTitle,
                    StartDate = source.StartDate,
                    EndDate = source.EndDate,
                    Description = source.Description
                };
            }
        }
    }

    public class ApplicationQuestionsSection
    {
        public SectionStatus SkillsAndStrengthsStatus { get; init; }
        public SectionStatus WhatInterestsYouStatus { get; init; }
        public Question? AdditionalQuestion1 { get; set; }
        public Question? AdditionalQuestion2 { get; set; }

        public static implicit operator ApplicationQuestionsSection(GetApplicationSummaryApiResponse.ApplicationQuestionsSection source)
        {
            return new ApplicationQuestionsSection
            {
                SkillsAndStrengthsStatus = source.SkillsAndStrengthsStatus,
                WhatInterestsYouStatus = source.WhatInterestsYouStatus,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
            };
        }

        public record Question
        {
            public Guid Id { get; set; }
            public SectionStatus Status { get; set; }
            public string? QuestionLabel { get; set; }
            public string? Answer { get; set; }

            public static implicit operator Question?(GetApplicationSummaryApiResponse.ApplicationQuestionsSection.Question? source)
            {
                if (source is null) return null;

                return new Question
                {
                    Id = source.Id,
                    Answer = source.Answer,
                    QuestionLabel = source.QuestionLabel,
                    Status = source.Status,
                };
            }
        }
    }

    public class InterviewAdjustmentsSection
    {
        public SectionStatus RequestAdjustmentsStatus { get; set; }
        public string? InterviewAdjustmentsDescription { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationSummaryApiResponse.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustmentsStatus = source.RequestAdjustmentsStatus,
                InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription
            };
        }
    }

    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfidentStatus { get; set; }
        public bool ApplyUnderDisabilityConfidentScheme { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationSummaryApiResponse.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfidentStatus = source.InterviewUnderDisabilityConfidentStatus,
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme ?? false
            };
        }
    }

    public record WhatIsYourInterestSection
    {
        public string? WhatIsYourInterest { get; set; }

        public static implicit operator WhatIsYourInterestSection(GetApplicationSummaryApiResponse.WhatIsYourInterestSection source)
        {
            return new WhatIsYourInterestSection
            {
                WhatIsYourInterest = source.WhatIsYourInterest
            };
        }
    }

    public record AboutYouSection
    {
        public string? SkillsAndStrengths { get; set; }
        public string? Improvements { get; set; }
        public string? HobbiesAndInterests { get; set; }
        public string? Support { get; set; }

        public static implicit operator AboutYouSection(GetApplicationSummaryApiResponse.AboutYouSection source)
        {
            return new AboutYouSection
            {
                Support = source.Support,
                HobbiesAndInterests = source.HobbiesAndInterests,
                Improvements = source.Improvements,
                SkillsAndStrengths = source.SkillsAndStrengths
            };
        }
    }
}