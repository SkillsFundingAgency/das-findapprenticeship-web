using SFA.DAS.FAA.Domain.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView
{
    public record GetApplicationViewQueryResult
    {
        public VacancyDetailsSection VacancyDetails { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public CandidateDetailsSection Candidate { get; init; }
        public AboutYouSection AboutYou { get; init; }
        public EducationHistorySection EducationHistory { get; init; }
        public WorkHistorySection WorkHistory { get; init; }
        public ApplicationQuestionsSection ApplicationQuestions { get; init; }
        public InterviewAdjustmentsSection InterviewAdjustments { get; init; }
        public DisabilityConfidenceSection DisabilityConfidence { get; init; }
        public WhatIsYourInterestSection WhatIsYourInterest { get; init; }
        public EmploymentLocationSection? EmploymentLocation { get; init; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public DateTime? MigrationDate { get; set; }
        public ApprenticeshipTypes? ApprenticeshipType { get; init; } = ApprenticeshipTypes.Standard;

        public static implicit operator GetApplicationViewQueryResult(GetApplicationViewApiResponse source)
        {
            Enum.TryParse<ApplicationStatus>(source.ApplicationStatus, true, out var applicationStatus);
            return new GetApplicationViewQueryResult
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
                EmploymentLocation = source.EmploymentLocation,
                VacancyDetails = source.VacancyDetails,
                ApplicationStatus = applicationStatus,
                WithdrawnDate = source.WithdrawnDate,
                MigrationDate = source.MigrationDate,
                ApprenticeshipType = source.ApprenticeshipType,
            };
        }

        public record VacancyDetailsSection
        {
            public string Title { get; set; }
            public string EmployerName { get; set; }

            public static implicit operator VacancyDetailsSection(GetApplicationViewApiResponse.VacancyDetailsSection source)
            {
                return new VacancyDetailsSection
                {
                    EmployerName = source.EmployerName,
                    Title = source.Title
                };
            }
        }

        public record EmploymentLocationSection : LocationDto
        {
            public static implicit operator EmploymentLocationSection?(GetApplicationViewApiResponse.EmploymentLocationSection? source)
            {
                if (source is null) return null;

                return new EmploymentLocationSection
                {
                    Id = source.Id,
                    Addresses = source.Addresses,
                    EmploymentLocationInformation = source.EmploymentLocationInformation,
                    EmployerLocationOption = source.EmployerLocationOption,
                };
            }
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

            public static implicit operator CandidateDetailsSection(GetApplicationViewApiResponse.CandidateDetailsSection source)
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

            public static implicit operator AddressDetailsSection?(GetApplicationViewApiResponse.AddressDetailsSection? source)
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
            public List<TrainingCourse?> TrainingCourses { get; set; } = [];
            public List<Qualification?> Qualifications { get; set; } = [];
            public List<QualificationReference?> QualificationTypes { get; set; } = [];

            public static implicit operator EducationHistorySection(GetApplicationViewApiResponse.EducationHistorySection source)
            {
                return new EducationHistorySection
                {
                    TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList(),
                    Qualifications = source.Qualifications.Select(x => (Qualification)x).ToList(),
                    QualificationTypes = source.QualificationTypes.Select(x => (QualificationReference)x).ToList()
                };
            }

            public record TrainingCourse
            {
                public string? CourseName { get; set; }
                public int YearAchieved { get; set; }

                public static implicit operator TrainingCourse?(GetApplicationViewApiResponse.EducationHistorySection.TrainingCourse? source)
                {
                    if (source is null) return null;

                    return new TrainingCourse
                    {
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

                public static implicit operator Qualification?(GetApplicationViewApiResponse.EducationHistorySection.Qualification? source)
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

                public static implicit operator QualificationReference?(GetApplicationViewApiResponse.EducationHistorySection.QualificationReference? source)
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
            public List<Job?> Jobs { get; set; } = [];
            public List<VolunteeringAndWorkExperience?> VolunteeringAndWorkExperiences { get; set; } = [];

            public static implicit operator WorkHistorySection(GetApplicationViewApiResponse.WorkHistorySection source)
            {
                return new WorkHistorySection
                {
                    Jobs = source.Jobs.Select(x => (Job)x).ToList(),
                    VolunteeringAndWorkExperiences = source.VolunteeringAndWorkExperiences.Select(x => (VolunteeringAndWorkExperience)x).ToList(),
                };
            }

            public record Job
            {
                public string? Employer { get; set; }
                public string? JobTitle { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public string? Description { get; set; }

                public static implicit operator Job?(GetApplicationViewApiResponse.WorkHistorySection.Job? source)
                {
                    if (source is null) return null;

                    return new Job
                    {
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
                public string? Employer { get; set; }
                public string? JobTitle { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public string? Description { get; set; }

                public static implicit operator VolunteeringAndWorkExperience?(GetApplicationViewApiResponse.WorkHistorySection.VolunteeringAndWorkExperience? source)
                {
                    if (source is null) return null;

                    return new VolunteeringAndWorkExperience
                    {
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
            public Question? AdditionalQuestion1 { get; set; }
            public Question? AdditionalQuestion2 { get; set; }

            public static implicit operator ApplicationQuestionsSection(GetApplicationViewApiResponse.ApplicationQuestionsSection source)
            {
                return new ApplicationQuestionsSection
                {
                    AdditionalQuestion1 = source.AdditionalQuestion1,
                    AdditionalQuestion2 = source.AdditionalQuestion2,
                };
            }

            public record Question
            {
                public string? QuestionLabel { get; set; }
                public string? Answer { get; set; }

                public static implicit operator Question?(GetApplicationViewApiResponse.ApplicationQuestionsSection.Question? source)
                {
                    if (source is null) return null;

                    return new Question
                    {
                        Answer = source.Answer,
                        QuestionLabel = source.QuestionLabel,
                    };
                }
            }
        }

        public class InterviewAdjustmentsSection
        {
            public string? InterviewAdjustmentsDescription { get; set; }

            public static implicit operator InterviewAdjustmentsSection(GetApplicationViewApiResponse.InterviewAdjustmentsSection source)
            {
                return new InterviewAdjustmentsSection
                {
                    InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription
                };
            }
        }

        public class DisabilityConfidenceSection
        {
            public bool ApplyUnderDisabilityConfidentScheme { get; set; }

            public static implicit operator DisabilityConfidenceSection(GetApplicationViewApiResponse.DisabilityConfidenceSection source)
            {
                return new DisabilityConfidenceSection
                {
                    ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme ?? false
                };
            }
        }

        public record WhatIsYourInterestSection
        {
            public string? WhatIsYourInterest { get; set; }

            public static implicit operator WhatIsYourInterestSection(GetApplicationViewApiResponse.WhatIsYourInterestSection source)
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
            public string? Support { get; set; }

            public static implicit operator AboutYouSection(GetApplicationViewApiResponse.AboutYouSection source)
            {
                return new AboutYouSection
                {
                    Support = source.Support,
                    SkillsAndStrengths = source.SkillsAndStrengths
                };
            }
        }
    }
}