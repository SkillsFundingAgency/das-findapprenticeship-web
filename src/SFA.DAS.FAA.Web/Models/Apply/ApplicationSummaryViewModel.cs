using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class ApplicationSummaryViewModel
{
    public Guid ApplicationId { get; set; }

    [BindProperty]
    public bool IsConsentProvided { get; set; }

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
            WhatIsYourInterest = source.WhatIsYourInterest,
            EmploymentLocation = source.EmploymentLocation,
            AboutYou = source.AboutYou,
            IsApplicationComplete = source.IsApplicationComplete
        };
    }

    public bool IsDisabilityConfident { get; init; }
    public bool IsApplicationComplete {get;init;}
    public bool ShowLocationSection => EmploymentLocation is {EmployerLocationOption: AvailableWhere.MultipleLocations};
    public CandidateDetailsSection Candidate { get; init; } = new();
    public EducationHistorySection EducationHistory { get; init; } = new();
    public WorkHistorySection WorkHistory { get; init; } = new();
    public ApplicationQuestionsSection ApplicationQuestions { get; init; } = new();
    public InterviewAdjustmentsSection InterviewAdjustments { get; init; } = new();
    public DisabilityConfidenceSection DisabilityConfidence { get; init; } = new();
    public WhatIsYourInterestSection WhatIsYourInterest { get; init; } = new();
    public EmploymentLocationSection? EmploymentLocation { get; init; } = new();
    public AboutYouSection AboutYou { get; init; } = new();


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

    public class EducationHistorySection
    {
        public SectionStatus QualificationsStatus { get; private init; }
        public SectionStatus TrainingCoursesStatus { get; private init; }
        public List<TrainingCourse> TrainingCourses { get; set; } = [];
        public QualificationsViewModel Qualifications { get; set; } = new();

        public static implicit operator EducationHistorySection(GetApplicationSummaryQueryResult.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                QualificationsStatus = source.QualificationsStatus,
                TrainingCoursesStatus = source.TrainingCoursesStatus,
                TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList(),
                Qualifications = QualificationsViewModel.MapFromQueryResult(source)

            };
        }

        public record TrainingCourse
        {
            public Guid Id { get; set; }
            public string? CourseName { get; set; }
            public int YearAchieved { get; set; }

            public static implicit operator TrainingCourse(GetApplicationSummaryQueryResult.EducationHistorySection.TrainingCourse source)
            {
                return new TrainingCourse
                {
                    Id = source.Id,
                    CourseName = source.CourseName,
                    YearAchieved = source.YearAchieved
                };
            }
        }

        public class QualificationsViewModel
        {
            public List<QualificationGroup> QualificationGroups { get; set; } = [];

            public bool ShowQualifications { get; set; }

            public class QualificationGroup
            {
                public string? DisplayName { get; set; }
                public List<Qualification> Qualifications { get; set; } = [];
                public bool AllowMultipleAdd { get; set; }
                public bool ShowAdditionalInformation { get; set; }
                public bool? ShowLevel { get; set; }
            }

            public class Qualification
            {
                public Guid? Id { get; set; }
                public string? Subject { get; set; }
                internal string? Grade { get; set; }
                public string? AdditionalInformation { get; set; }
                public short? QualificationOrder { get; set; }
                internal bool? IsPredicted { get; set; }
                public string? GradeLabel => IsPredicted is true ? $"{Grade} (predicted)" : Grade;
            }

            public static QualificationsViewModel MapFromQueryResult(GetApplicationSummaryQueryResult.EducationHistorySection source)
            {
                var result = new QualificationsViewModel
                {
                    ShowQualifications = source.Qualifications.Count != 0
                };

                var viewModelQualificationTypes =
                    source.QualificationTypes.Select(c => new QualificationDisplayTypeViewModel(c?.Name!, c!.Id)).ToList();

                foreach (var qualificationType in viewModelQualificationTypes.OrderBy(x => x.ListOrder))
                {
                    if (source.Qualifications.Any(x => x?.QualificationReferenceId == qualificationType.Id))
                    {
                        if (qualificationType.AllowMultipleAdd)
                        {
                            var group = MapGroup(qualificationType,
                                source.Qualifications.Where(x => x.QualificationReferenceId == qualificationType.Id));
                            result.QualificationGroups.Add(group);
                        }
                        else
                        {
                            foreach (var qualification in source.Qualifications.Where(x =>
                                         x?.QualificationReferenceId == qualificationType.Id))
                            {
                                var group = MapGroup(qualificationType, new[] { qualification }!);
                                result.QualificationGroups.Add(group);
                            }
                        }
                    }
                }

                return result;
            }

            private static QualificationGroup MapGroup(QualificationDisplayTypeViewModel qualificationType,
                IEnumerable<GetApplicationSummaryQueryResult.EducationHistorySection.Qualification> qualifications)
            {
                var result = new QualificationGroup
                {
                    DisplayName = qualificationType.GroupTitle,
                    ShowAdditionalInformation = qualificationType.ShouldDisplayAdditionalInformationField,
                    AllowMultipleAdd = qualificationType.AllowMultipleAdd,
                    ShowLevel = qualificationType.CanShowLevel,
                    Qualifications = qualifications
                        .Select(x => new Qualification
                        {
                            Id = x.Id,
                            Subject = x.Subject != null && x.Subject.Contains('|') ? x.Subject.Split('|')[1] : x.Subject,
                            Grade = x.Grade,
                            QualificationOrder = x.QualificationOrder,
                            AdditionalInformation = x.AdditionalInformation,
                            IsPredicted = x.IsPredicted
                        }).OrderBy(ord => ord.QualificationOrder).ToList()
                };

                return result;
            }

        }
    }

    public record EmploymentLocationSection : LocationDto
    {
        public SectionStatus EmploymentLocationStatus { get; private init; }
        public static implicit operator EmploymentLocationSection?(GetApplicationSummaryQueryResult.EmploymentLocationSection? source)
        {
            if (source is null) return null;
            return new EmploymentLocationSection
            {
                Id = source.Id,
                Addresses = source.Addresses.Where(x => x.IsSelected).OrderBy(x => x.AddressOrder).ToList(),
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                EmployerLocationOption = source.EmployerLocationOption,
                EmploymentLocationStatus = source.EmploymentLocationStatus
            };
        }
    }

    public class WorkHistorySection
    {
        public SectionStatus JobsStatus { get; private init; }
        public SectionStatus VolunteeringAndWorkExperienceStatus { get; private init; }
        public List<Job> Jobs { get; set; } = [];
        public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = [];

        public static implicit operator WorkHistorySection(GetApplicationSummaryQueryResult.WorkHistorySection source)
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
            public string JobHeader => $"{JobTitle}, {Employer}";
            private string? Employer { get; set; }
            private string? JobTitle { get; set; }
            public string? Description { get; set; }
            public string? JobDates { get; private init; }

            public static implicit operator Job(GetApplicationSummaryQueryResult.WorkHistorySection.Job source)
            {
                return new Job
                {
                    Id = source.Id,
                    JobTitle = source.JobTitle,
                    Employer = source.Employer,
                    Description = source.Description,
                    JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
                };
            }
        }
        public record VolunteeringAndWorkExperience
        {
            public Guid Id { get; set; }
            public string? Employer { get; set; }
            public string? Description { get; set; }
            public string? JobDates { get; private init; }

            public static implicit operator VolunteeringAndWorkExperience(GetApplicationSummaryQueryResult.WorkHistorySection.VolunteeringAndWorkExperience source)
            {
                return new VolunteeringAndWorkExperience
                {
                    Id = source.Id,
                    Employer = source.Employer,
                    Description = source.Description,
                    JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
                };
            }
        }
    }

    public class ApplicationQuestionsSection
    {
        public SectionStatus SkillsAndStrengthsStatus { get; set; }
        public SectionStatus WhatInterestsYouStatus { get; set; }
        public Question? AdditionalQuestion1 { get; set; }
        public Question? AdditionalQuestion2 { get; set; }

        public static implicit operator ApplicationQuestionsSection(GetApplicationSummaryQueryResult.ApplicationQuestionsSection source)
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

            public static implicit operator Question?(GetApplicationSummaryQueryResult.ApplicationQuestionsSection.Question? source)
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
        public SectionStatus RequestAdjustmentsStatus { get; private init; }
        public bool IsSupportRequestRequired { get; set; }
        public string? InterviewAdjustmentsDescription { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationSummaryQueryResult.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                RequestAdjustmentsStatus = source.RequestAdjustmentsStatus,
                InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription,
                IsSupportRequestRequired = !string.IsNullOrEmpty(source.InterviewAdjustmentsDescription)
            };
        }
    }

    public class DisabilityConfidenceSection
    {
        public SectionStatus InterviewUnderDisabilityConfidentStatus { get; private init; }
        public bool ApplyUnderDisabilityConfidentScheme { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationSummaryQueryResult.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                InterviewUnderDisabilityConfidentStatus = source.InterviewUnderDisabilityConfidentStatus,
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme
            };
        }
    }

    public record WhatIsYourInterestSection
    {
        public string? WhatIsYourInterest { get; set; }

        public static implicit operator WhatIsYourInterestSection(GetApplicationSummaryQueryResult.WhatIsYourInterestSection source)
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

        public static implicit operator AboutYouSection(GetApplicationSummaryQueryResult.AboutYouSection source)
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