using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Globalization;

namespace SFA.DAS.FAA.Web.Models.Applications;

public class ApplicationViewModel
{
    public Guid ApplicationId { get; set; }

    public static implicit operator ApplicationViewModel(GetApplicationViewQueryResult source)
    {
        return new ApplicationViewModel
        {
            Candidate = source.Candidate,
            ApplicationQuestions = source.ApplicationQuestions,
            DisabilityConfidence = source.DisabilityConfidence,
            EducationHistory = source.EducationHistory,
            EmploymentLocation = source.EmploymentLocation,
            InterviewAdjustments = source.InterviewAdjustments,
            WorkHistory = source.WorkHistory,
            IsDisabilityConfident = source.IsDisabilityConfident,
            WhatIsYourInterest = source.WhatIsYourInterest,
            AboutYou = source.AboutYou,
            VacancyDetails = source.VacancyDetails,
            ApplicationStatus = source.ApplicationStatus,
            WithdrawnDate = source.WithdrawnDate,
            MigrationDate = source.MigrationDate,
        };
    }

    public string? BannerMessage => GetBannerMessage();
    public bool ShowLocationSection => EmploymentLocation is { EmployerLocationOption: AvailableWhere.MultipleLocations, EmploymentAddress.Count: > 0 };
    public ApplicationStatus ApplicationStatus { get; set; }
    public DateTime? WithdrawnDate { get; set; }
    public DateTime? MigrationDate { get; set; }

    public bool IsDisabilityConfident { get; init; }
    public CandidateDetailsSection Candidate { get; init; } = new();
    public EducationHistorySection EducationHistory { get; init; } = new();
    public WorkHistorySection WorkHistory { get; init; } = new();
    public ApplicationQuestionsSection ApplicationQuestions { get; init; } = new();
    public InterviewAdjustmentsSection InterviewAdjustments { get; init; } = new();
    public DisabilityConfidenceSection DisabilityConfidence { get; init; } = new();
    public WhatIsYourInterestSection WhatIsYourInterest { get; init; } = new();
    public AboutYouSection AboutYou { get; init; } = new();
    public VacancyDetailsSection VacancyDetails { get; init; } = new();
    public EmploymentLocationSection? EmploymentLocation { get; init; } = new();

    public record VacancyDetailsSection
    {
        public string? Title { get; set; }
        public string? EmployerName { get; set; }

        public static implicit operator VacancyDetailsSection(GetApplicationViewQueryResult.VacancyDetailsSection source)
        {
            return new VacancyDetailsSection
            {
                EmployerName = source.EmployerName,
                Title = source.Title
            };
        }
    }

    public record EmploymentLocationSection
    {
        public List<EmploymentLocationViewModel>? EmploymentAddress { get; init; }
        public AvailableWhere? EmployerLocationOption { get; set; }

        public static implicit operator EmploymentLocationSection?(GetApplicationViewQueryResult.EmploymentLocationSection? source)
        {
            if (source is null) return null;

            var addresses = source.Addresses
                .Where(x => x.IsSelected)
                .Select(x => (EmploymentLocationViewModel)x)
                .OrderBy(add => add.AddressOrder)
                .ToList();

            return new EmploymentLocationSection
            {
                EmploymentAddress = addresses,
                EmployerLocationOption = source.EmployerLocationOption,
            };
        }
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
        public AddressDetailsSection? Address { get; init; } = new();

        public static implicit operator CandidateDetailsSection(GetApplicationViewQueryResult.CandidateDetailsSection source)
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

        public static implicit operator AddressDetailsSection?(GetApplicationViewQueryResult.AddressDetailsSection? source)
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
        public List<TrainingCourse> TrainingCourses { get; set; } = [];
        public QualificationsViewModel Qualifications { get; set; } = new();

        public static implicit operator EducationHistorySection(GetApplicationViewQueryResult.EducationHistorySection source)
        {
            return new EducationHistorySection
            {
                TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList(),
                Qualifications = QualificationsViewModel.MapFromQueryResult(source)
            };
        }

        public record TrainingCourse
        {
            public string? CourseName { get; set; }
            public int YearAchieved { get; set; }

            public static implicit operator TrainingCourse(GetApplicationViewQueryResult.EducationHistorySection.TrainingCourse source)
            {
                return new TrainingCourse
                {
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
                internal bool? IsPredicted { get; set; }
                public short? QualificationOrder { get; set; }
                public string? GradeLabel => IsPredicted is true ? $"{Grade} (predicted)" : Grade;
            }

            public static QualificationsViewModel MapFromQueryResult(GetApplicationViewQueryResult.EducationHistorySection source)
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
                IEnumerable<GetApplicationViewQueryResult.EducationHistorySection.Qualification> qualifications)
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
                            AdditionalInformation = x.AdditionalInformation,
                            QualificationOrder = x.QualificationOrder,
                            IsPredicted = x.IsPredicted
                        }).OrderBy(ord => ord.QualificationOrder).ToList()
                };

                return result;
            }

        }
    }

    public class WorkHistorySection
    {
        public List<Job> Jobs { get; set; } = [];
        public List<VolunteeringAndWorkExperience> VolunteeringAndWorkExperiences { get; set; } = [];

        public static implicit operator WorkHistorySection(GetApplicationViewQueryResult.WorkHistorySection source)
        {
            return new WorkHistorySection
            {
                Jobs = source.Jobs.Select(x => (Job)x).ToList(),
                VolunteeringAndWorkExperiences = source.VolunteeringAndWorkExperiences.Select(x => (VolunteeringAndWorkExperience)x).ToList(),
            };
        }

        public record Job
        {
            public string JobHeader => $"{JobTitle}, {Employer}";
            private string? Employer { get; set; }
            private string? JobTitle { get; set; }
            public string? Description { get; set; }
            public string? JobDates { get; private init; }

            public static implicit operator Job(GetApplicationViewQueryResult.WorkHistorySection.Job source)
            {
                return new Job
                {
                    JobTitle = source.JobTitle,
                    Employer = source.Employer,
                    Description = source.Description,
                    JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
                };
            }
        }
        public record VolunteeringAndWorkExperience
        {
            public string? Employer { get; set; }
            public string? Description { get; set; }
            public string? JobDates { get; private init; }

            public static implicit operator VolunteeringAndWorkExperience(GetApplicationViewQueryResult.WorkHistorySection.VolunteeringAndWorkExperience source)
            {
                return new VolunteeringAndWorkExperience
                {
                    Employer = source.Employer,
                    Description = source.Description,
                    JobDates = source.EndDate is null ? $"{source.StartDate:MMMM yyyy} onwards" : $"{source.StartDate:MMMM yyyy} to {source.EndDate:MMMM yyyy}"
                };
            }
        }
    }

    public class ApplicationQuestionsSection
    {
        public Question? AdditionalQuestion1 { get; set; }
        public Question? AdditionalQuestion2 { get; set; }

        public static implicit operator ApplicationQuestionsSection(GetApplicationViewQueryResult.ApplicationQuestionsSection source)
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

            public static implicit operator Question?(GetApplicationViewQueryResult.ApplicationQuestionsSection.Question? source)
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
        public bool IsSupportRequestRequired { get; set; }
        public string? InterviewAdjustmentsDescription { get; set; }

        public static implicit operator InterviewAdjustmentsSection(GetApplicationViewQueryResult.InterviewAdjustmentsSection source)
        {
            return new InterviewAdjustmentsSection
            {
                InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription,
                IsSupportRequestRequired = !string.IsNullOrEmpty(source.InterviewAdjustmentsDescription)
            };
        }
    }

    public class DisabilityConfidenceSection
    {
        public bool ApplyUnderDisabilityConfidentScheme { get; set; }

        public static implicit operator DisabilityConfidenceSection(GetApplicationViewQueryResult.DisabilityConfidenceSection source)
        {
            return new DisabilityConfidenceSection
            {
                ApplyUnderDisabilityConfidentScheme = source.ApplyUnderDisabilityConfidentScheme
            };
        }
    }

    public record WhatIsYourInterestSection
    {
        public string? WhatIsYourInterest { get; set; }

        public static implicit operator WhatIsYourInterestSection(GetApplicationViewQueryResult.WhatIsYourInterestSection source)
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

        public static implicit operator AboutYouSection(GetApplicationViewQueryResult.AboutYouSection source)
        {
            return new AboutYouSection
            {
                Support = source.Support,
                SkillsAndStrengths = source.SkillsAndStrengths
            };
        }
    }

    private string GetBannerMessage()
    {
        return ApplicationStatus switch
        {
            ApplicationStatus.Withdrawn =>
                $"You withdrew your application for this apprenticeship on {WithdrawnDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}.",
            _ => string.Empty
        };
    }
}