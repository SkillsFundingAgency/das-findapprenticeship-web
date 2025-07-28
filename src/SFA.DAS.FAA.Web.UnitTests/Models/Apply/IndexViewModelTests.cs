using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply
{
    [TestFixture]
    public class IndexViewModelTests
    {
        [Test, MoqAutoData]
        public void Map_Returns_Expected_Result(
            GetIndexRequest request,
            Mock<IDateTimeService> dateTimeService,
            GetIndexQueryResult source)
        {
            var result = IndexViewModel.Map(dateTimeService.Object, request, source);
            var isVacancyClosedEarly = source.ClosedDate.HasValue && source.ClosedDate < source.ClosingDate;

            using var scope = new AssertionScope();
            result.VacancyReference.Should().Be(source.VacancyReference);
            result.VacancyTitle.Should().Be(source.VacancyTitle);
            result.EmployerName.Should().Be(source.EmployerName);
            result.ClosingDate.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, source.ClosingDate));
            result.ClosedDate.Should().Be(VacancyDetailsHelperService.GetClosedDate(source.ClosedDate, isVacancyClosedEarly));
            result.IsVacancyClosedEarly.Should().Be(isVacancyClosedEarly);
            result.IsVacancyClosed.Should().BeTrue();
            result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            result.EducationHistory.Qualifications.Should().Be(source.EducationHistory.Qualifications);
            result.EducationHistory.TrainingCourses.Should().Be(source.EducationHistory.TrainingCourses);
            result.WorkHistory.Jobs.Should().Be(source.WorkHistory.Jobs);
            result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(source.WorkHistory.VolunteeringAndWorkExperience);
            result.ApplicationQuestions.SkillsAndStrengths.Should().Be(source.ApplicationQuestions.SkillsAndStrengths);
            result.ApplicationQuestions.AdditionalQuestion1.Should().Be(source.ApplicationQuestions.AdditionalQuestion1);
            result.ApplicationQuestions.AdditionalQuestion2.Should().Be(source.ApplicationQuestions.AdditionalQuestion2);
            result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(source.ApplicationQuestions.AdditionalQuestion1Label);
            result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(source.ApplicationQuestions.AdditionalQuestion2Label);
            result.ApplicationQuestions.WhatInterestsYou.Should().Be(source.ApplicationQuestions.WhatInterestsYou);
            result.InterviewAdjustments.RequestAdjustments.Should().Be(source.InterviewAdjustments.RequestAdjustments);
            result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(source.DisabilityConfidence.InterviewUnderDisabilityConfident);
            result.ApplicationId.Should().Be(request.ApplicationId);
        }

        [Test, MoqAutoData]
        public void Then_Closing_Date_Is_Null_Map_Returns_Expected_Result(
            GetIndexRequest request,
            Mock<IDateTimeService> dateTimeService,
            GetIndexQueryResult source)
        {
            source.ClosedDate = null;
            var result = IndexViewModel.Map(dateTimeService.Object, request, source);

            using var scope = new AssertionScope();
            result.VacancyReference.Should().Be(source.VacancyReference);
            result.VacancyTitle.Should().Be(source.VacancyTitle);
            result.EmployerName.Should().Be(source.EmployerName);
            result.ClosingDate.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, source.ClosingDate));
            result.ClosedDate.Should().BeNullOrEmpty();
            result.IsVacancyClosed.Should().BeFalse();
            result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            result.EducationHistory.Qualifications.Should().Be(source.EducationHistory.Qualifications);
            result.EducationHistory.TrainingCourses.Should().Be(source.EducationHistory.TrainingCourses);
            result.WorkHistory.Jobs.Should().Be(source.WorkHistory.Jobs);
            result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(source.WorkHistory.VolunteeringAndWorkExperience);
            result.ApplicationQuestions.SkillsAndStrengths.Should().Be(source.ApplicationQuestions.SkillsAndStrengths);
            result.ApplicationQuestions.AdditionalQuestion1.Should().Be(source.ApplicationQuestions.AdditionalQuestion1);
            result.ApplicationQuestions.AdditionalQuestion2.Should().Be(source.ApplicationQuestions.AdditionalQuestion2);
            result.ApplicationQuestions.AdditionalQuestion1Label.Should().Be(source.ApplicationQuestions.AdditionalQuestion1Label);
            result.ApplicationQuestions.AdditionalQuestion2Label.Should().Be(source.ApplicationQuestions.AdditionalQuestion2Label);
            result.ApplicationQuestions.WhatInterestsYou.Should().Be(source.ApplicationQuestions.WhatInterestsYou);
            result.InterviewAdjustments.RequestAdjustments.Should().Be(source.InterviewAdjustments.RequestAdjustments);
            result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(source.DisabilityConfidence.InterviewUnderDisabilityConfident);
            result.ApplicationId.Should().Be(request.ApplicationId);
        }
    }
}
