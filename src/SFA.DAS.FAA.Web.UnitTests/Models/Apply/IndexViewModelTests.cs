using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetIndex;
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
            Mock<IDateTimeService> dateTimeService,
            GetIndexQueryResult source)
        {
            var result = IndexViewModel.Map(dateTimeService.Object, source);

            using var scope = new AssertionScope();
            result.VacancyTitle.Should().Be(source.VacancyTitle);
            result.EmployerName.Should().Be(source.EmployerName);
            result.ClosingDate.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, source.ClosingDate));
            result.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
            result.EducationHistory.Qualifications.Should().Be(source.EducationHistory.Qualifications);
            result.EducationHistory.TrainingCourses.Should().Be(source.EducationHistory.TrainingCourses);
            result.WorkHistory.Jobs.Should().Be(source.WorkHistory.Jobs);
            result.WorkHistory.VolunteeringAndWorkExperience.Should().Be(source.WorkHistory.VolunteeringAndWorkExperience);
            result.ApplicationQuestions.SkillsAndStrengths.Should().Be(source.ApplicationQuestions.SkillsAndStrengths);
            result.ApplicationQuestions.Travel.Should().Be(source.ApplicationQuestions.Travel);
            result.ApplicationQuestions.WhatInterestsYou.Should().Be(source.ApplicationQuestions.WhatInterestsYou);
            result.InterviewAdjustments.RequestAdjustments.Should().Be(source.InterviewAdjustments.RequestAdjustments);
            result.DisabilityConfidence.InterviewUnderDisabilityConfident.Should().Be(source.DisabilityConfidence.InterviewUnderDisabilityConfident);
        }
    }
}
