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
    // [Test, MoqAutoData]
    // public void Then_The_Fields_Are_Mapped(GetApprenticeshipVacancyQueryResult vacancies, [Frozen] Mock <IDateTimeService> dateTimeService)
    // {
    //     var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, vacancies);
    //
    //     actual.Should().BeEquivalentTo(vacancies.Vacancy, options=> options
    //         .Excluding(c=>c.ClosingDateLabel)
    //         .Excluding(c=>c.PostedDate)
    //         .Excluding(c=>c.Id)
    //         .Excluding(c=>c.CourseTitle)
    //         .Excluding(c=>c.Address.Postcode)
    //     );
    //     actual.CourseTitle.Should().Be($"{vacancies.Vacancy.CourseTitle} (level {vacancies.Vacancy.CourseLevel})");
    //     //actual.VacancyPostCode.Should().Be(vacancies.Postcode);
    // }

    [Test]
    [MoqInlineAutoData(null, "0 hours a week")]
    [MoqInlineAutoData("0", "0 hours a week")]
    [MoqInlineAutoData("37.5", "37 hours 30 minutes a week")]
    [MoqInlineAutoData("37.25", "37 hours 15 minutes a week")]
    public void Then_The_HoursPerWeek_Is_Shown_Correctly(
        string duration,
        string workingHours,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        if (result.Vacancy != null) result.Vacancy.HoursPerWeek = Convert.ToDecimal(duration);

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result);

        actual.HoursPerWeek.Should().Be(workingHours);
    }

    [Test, MoqAutoData]
    public void Then_The_ClosingDate_Less_than_31Days_Is_Shown_Correctly(
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2000, 01, 01));
        var dateLessThan31Days = new DateTime(2000, 02, 01);
        if (result.Vacancy != null) result.Vacancy.ClosingDate = dateLessThan31Days;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result);

        Assert.That(actual.ClosingDate, Is.Not.Null);
        actual.ClosingDate.Should().Contain("Closes in");
    }

    [Test, MoqAutoData]
    public void Then_The_ClosingDate_More_than_31Days_Is_Shown_Correctly(
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2000, 01, 01));

        var dateMoreThan31Days = new DateTime(2000, 04, 01);
        if (result.Vacancy != null) result.Vacancy.ClosingDate = dateMoreThan31Days;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result);

        Assert.That(actual.ClosingDate, Is.Not.Null);
        actual.ClosingDate.Should().Contain("Closes on");
    }
}