﻿using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class IndexViewModel
    {
        public static IndexViewModel Map(IDateTimeService dateTimeService, GetIndexRequest request, GetIndexQueryResult source)
        {
            return new IndexViewModel
            {
                VacancyReference = request.VacancyReference,
                ShowAccountCreatedBanner = false,
                VacancyTitle = source.VacancyTitle,
                EmployerName = source.EmployerName,
                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosingDate),
                IsDisabilityConfident = source.IsDisabilityConfident,
                EducationHistory = source.EducationHistory,
                WorkHistory = source.WorkHistory,
                ApplicationQuestions = source.ApplicationQuestions,
                InterviewAdjustments = source.InterviewAdjustments,
                DisabilityConfidence = source.DisabilityConfidence
            };
        }

        public string VacancyReference { get; set; }
        public bool ShowAccountCreatedBanner { get; set; }
        public string VacancyTitle { get; set; }
        public string EmployerName { get; set; }
        public string ClosingDate { get; set; }
        public bool IsDisabilityConfident { get; set; }

        public bool IsApplicationComplete => EducationHistory.TrainingCourses == SectionStatus.Completed &&
                                             EducationHistory.Qualifications == SectionStatus.Completed &&
                                             WorkHistory.VolunteeringAndWorkExperience == SectionStatus.Completed &&
                                             WorkHistory.Jobs == SectionStatus.Completed &&
                                             (!ApplicationQuestions.ShowAdditionalQuestion1 || ApplicationQuestions.AdditionalQuestion1 == SectionStatus.Completed) &&
                                             (!ApplicationQuestions.ShowAdditionalQuestion2 || ApplicationQuestions.AdditionalQuestion2 == SectionStatus.Completed) &&
                                             InterviewAdjustments.RequestAdjustments == SectionStatus.Completed &&
                                             (!IsDisabilityConfident || DisabilityConfidence.InterviewUnderDisabilityConfident == SectionStatus.Completed);

        public EducationHistorySection EducationHistory { get; set; } = new();
        public WorkHistorySection WorkHistory { get; set; } = new();
        public ApplicationQuestionsSection ApplicationQuestions { get; set; } = new();
        public InterviewAdjustmentsSection InterviewAdjustments { get; set; } = new();
        public DisabilityConfidenceSection DisabilityConfidence { get; set; } = new();

        public class EducationHistorySection
        {
            public SectionStatus Qualifications { get; set; }
            public SectionStatus TrainingCourses { get; set; }

            public static implicit operator EducationHistorySection(GetIndexQueryResult.EducationHistorySection source)
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

            public static implicit operator WorkHistorySection(GetIndexQueryResult.WorkHistorySection source)
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
            public string AdditionalQuestion1Label { get; set; }
            public string AdditionalQuestion2Label { get; set; }
            public bool ShowAdditionalQuestion1 => !string.IsNullOrWhiteSpace(AdditionalQuestion1Label);
            public bool ShowAdditionalQuestion2 => !string.IsNullOrWhiteSpace(AdditionalQuestion2Label);

            public static implicit operator ApplicationQuestionsSection(GetIndexQueryResult.ApplicationQuestionsSection source)
            {
                return new ApplicationQuestionsSection
                {
                    SkillsAndStrengths = source.SkillsAndStrengths,
                    WhatInterestsYou = source.WhatInterestsYou,
                    AdditionalQuestion1 = source.AdditionalQuestion1,
                    AdditionalQuestion2 = source.AdditionalQuestion2,
                    AdditionalQuestion1Label = source.AdditionalQuestion1Label,
                    AdditionalQuestion2Label = source.AdditionalQuestion2Label
                };
            }
        }

        public class InterviewAdjustmentsSection
        {
            public SectionStatus RequestAdjustments { get; set; }

            public static implicit operator InterviewAdjustmentsSection(GetIndexQueryResult.InterviewAdjustmentsSection source)
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

            public static implicit operator DisabilityConfidenceSection(GetIndexQueryResult.DisabilityConfidenceSection source)
            {
                return new DisabilityConfidenceSection
                {
                    InterviewUnderDisabilityConfident = source.InterviewUnderDisabilityConfident
                };
            }
        }
    }
}