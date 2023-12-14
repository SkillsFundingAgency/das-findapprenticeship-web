using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingVacancyDetailsViewModel
{
    [Test]
    [MoqInlineAutoData(null, "0 hours")]
    [MoqInlineAutoData("0", "0 hours")]
    [MoqInlineAutoData("37.5", "37 hours 30 minutes")]
    [MoqInlineAutoData("37.25", "37 hours 15 minutes")]
    public void Then_The_HoursPerWeek_Is_Shown_Correctly(
        string duration,
        string workingHours,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.Vacancy.HoursPerWeek = Convert.ToDecimal(duration);

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result);

        actual.HoursPerWeek.Should().Be(workingHours);
    }

    [Test, MoqAutoData]
    public void Then_The_ClosingDate_Less_than_31Days_Is_Shown_Correctly(
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);
        var dateLessThan31Days = DateTime.UtcNow.AddDays(-30);
        result.Vacancy.ClosingDate = dateLessThan31Days;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result);

        Assert.That(actual.ClosingDate, Is.Not.Null);
        actual.ClosingDate.Should().Contain("Closes in");
    }

    [Test, MoqAutoData]
    public void Then_The_ClosingDate_More_than_31Days_Is_Shown_Correctly(
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);

        var dateMoreThan31Days = DateTime.UtcNow.AddDays(60);
        result.Vacancy.ClosingDate = dateMoreThan31Days;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result);

        Assert.That(actual.ClosingDate, Is.Not.Null);
        actual.ClosingDate.Should().Contain("Closes on");
    }
}