using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

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

            using var scope = new AssertionScope();
            result.VacancyReference.Should().Be(request.VacancyReference);
            result.VacancyTitle.Should().Be(source.VacancyTitle);
            result.EmployerName.Should().Be(source.EmployerName);
            result.ClosingDate.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, source.ClosingDate));
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
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsApplicationComplete_Is_Set_Correctly(bool isComplete)
        {
            var viewModel = CreateViewModel(isComplete);
            viewModel.IsApplicationComplete.Should().Be(isComplete);
        }

        [Test]
        public void IsApplicationComplete_Is_Set_Correctly_With_No_Additional_Questions()
        {
            var viewModel = CreateViewModel();
            viewModel.ApplicationQuestions.AdditionalQuestion1Label = string.Empty;
            viewModel.ApplicationQuestions.AdditionalQuestion2Label = string.Empty;
            viewModel.ApplicationQuestions.AdditionalQuestion1 = SectionStatus.NotStarted;
            viewModel.ApplicationQuestions.AdditionalQuestion2 = SectionStatus.NotStarted;

            viewModel.IsApplicationComplete.Should().Be(true);
        }

        [Test]
        public void IsApplicationComplete_Is_Set_Correctly_Without_Disability()
        {
            var viewModel = CreateViewModel();
            viewModel.IsDisabilityConfident = false;
            viewModel.DisabilityConfidence.InterviewUnderDisabilityConfident = SectionStatus.NotStarted;

            viewModel.IsApplicationComplete.Should().Be(true);
        }

        private IndexViewModel CreateViewModel(bool isComplete = true)
        {
            var status = isComplete ? SectionStatus.Completed : SectionStatus.NotStarted;

            var viewModel = new IndexViewModel();
            viewModel.EducationHistory.TrainingCourses = status;
            viewModel.EducationHistory.Qualifications = status;
            viewModel.IsDisabilityConfident = true;
            viewModel.DisabilityConfidence.InterviewUnderDisabilityConfident = status;
            viewModel.InterviewAdjustments.RequestAdjustments = status;
            viewModel.WorkHistory.VolunteeringAndWorkExperience = status;
            viewModel.WorkHistory.Jobs = status;
            viewModel.ApplicationQuestions.AdditionalQuestion1 = status;
            viewModel.ApplicationQuestions.AdditionalQuestion2 = status;
            viewModel.ApplicationQuestions.AdditionalQuestion1Label = "x";
            viewModel.ApplicationQuestions.AdditionalQuestion2Label = "x";
            return viewModel;
        }
    }
}
