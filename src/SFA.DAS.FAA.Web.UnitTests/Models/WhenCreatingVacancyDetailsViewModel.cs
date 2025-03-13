using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Globalization;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingVacancyDetailsViewModel
{
    [Test, MoqAutoData]
    public void Then_The_Fields_Are_Mapped(GetApprenticeshipVacancyQueryResult source, string mapsId, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var expected = new {
            Title = source.Vacancy!.Title,
            VacancyReference = source.Vacancy?.VacancyReference,
            VacancySummary = source.Vacancy?.Description,
            AnnualWage = VacancyDetailsHelperService.GetVacancyAdvertWageText(source.Vacancy, source.Vacancy.CandidateDateOfBirth),
            HoursPerWeek = source.Vacancy?.HoursPerWeek.ToString().GetWorkingHours(),
            Duration = source.Vacancy?.ExpectedDuration!.ToLower(),
            PositionsAvailable = source.Vacancy?.NumberOfPositions,
            WorkDescription = source.Vacancy?.LongDescription,
            ThingsToConsider = source.Vacancy?.ThingsToConsider,
            ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, source.Vacancy.ClosingDate, !string.IsNullOrEmpty(source.Vacancy.ApplicationUrl)),
            PostedDate = source.Vacancy.PostedDate.GetPostedDate(),
            StartDate = source.Vacancy.StartDate.GetStartDate(),
            WorkLocation = source.Vacancy.Address,
            WorkingPattern = source.Vacancy?.WorkingWeek,
            TrainingProviderName = source.Vacancy?.ProviderName,
            TrainingPlan = source.Vacancy?.TrainingDescription,
            OutcomeDescription = source.Vacancy?.OutcomeDescription,
            Skills = source.Vacancy?.Skills?.ToList(),
            EmployerWebsite =
                    VacancyDetailsHelperService.FormatEmployerWebsiteUrl(source.Vacancy?.EmployerWebsiteUrl),
            EmployerDescription = source.Vacancy?.EmployerDescription,
            EmployerName = source.Vacancy?.EmployerName,
            ContactOrganisationName = string.IsNullOrWhiteSpace(source.Vacancy?.EmployerContactName) ? source.Vacancy!.ProviderName : source.Vacancy!.EmployerName,
            ContactName = source.Vacancy?.EmployerContactName ?? source.Vacancy?.ProviderContactName,
            ContactEmail = source.Vacancy?.EmployerContactEmail ?? source.Vacancy?.ProviderContactEmail,
            ContactPhone = source.Vacancy?.EmployerContactPhone ?? source.Vacancy?.ProviderContactPhone,
            CourseTitle = $"{source.Vacancy?.CourseTitle} (level {source.Vacancy?.CourseLevel})",
            EssentialQualifications = source.Vacancy?.Qualifications?
                .Where(fil => fil.Weighting == Weighting.Essential).Select(l => (Qualification)l).ToList(),
            DesiredQualifications = source.Vacancy?.Qualifications?.Where(fil => fil.Weighting == Weighting.Desired)
                .Select(l => (Qualification)l).ToList(),
            CourseSkills = source.Vacancy?.CourseSkills,
            CourseCoreDuties = source.Vacancy?.CourseCoreDuties,
            CourseOverviewOfRole = source.Vacancy?.CourseOverviewOfRole,
            StandardPageUrl = source.Vacancy?.StandardPageUrl,
            IsDisabilityConfident = source.Vacancy is { IsDisabilityConfident: true },
            CourseLevels = source.Vacancy?.Levels,
            CourseLevelMapper = int.TryParse(source.Vacancy?.CourseLevel, out _) && source.Vacancy.Levels?.Count > 0
                    ? source.Vacancy?.Levels.FirstOrDefault(le => le.Code == Convert.ToInt16(source.Vacancy?.CourseLevel))?.Name
                    : string.Empty,
            IsClosed = source.Vacancy?.IsClosed ?? false,
            ClosedDate = $"This apprenticeship closed on {source.Vacancy?.ClosingDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture) ?? string.Empty}.",
            ApplicationUrl = $"https://{source.Vacancy.ApplicationUrl}",
            GoogleMapsId = mapsId,
            EmploymentLocationInformation = source.Vacancy.EmploymentLocationInformation
        };

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, source, mapsId);

        actual.Should().BeEquivalentTo(expected);
        
    }

    [Test]
    [MoqInlineAutoData(null, "0 hours a week")]
    [MoqInlineAutoData("0", "0 hours a week")]
    [MoqInlineAutoData("37.5", "37 hours 30 minutes a week")]
    [MoqInlineAutoData("37.25", "37 hours 15 minutes a week")]
    public void Then_The_HoursPerWeek_Is_Shown_Correctly(
        string duration,
        string workingHours,
        string mapsId,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        if (result.Vacancy != null) result.Vacancy.HoursPerWeek = Convert.ToDecimal(duration);

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result, mapsId);

        actual.HoursPerWeek.Should().Be(workingHours);
        actual.GoogleMapsId.Should().Be(mapsId);
    }

    [Test, MoqAutoData]
    public void Then_The_ClosingDate_Less_than_31Days_Is_Shown_Correctly(
        string mapsId,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2000, 01, 01));
        var dateLessThan31Days = new DateTime(2000, 02, 01);
        if (result.Vacancy != null) result.Vacancy.ClosingDate = dateLessThan31Days;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result, mapsId);

        Assert.That(actual.ClosingDate, Is.Not.Null);
        actual.ClosingDate.Should().Contain("Closes in");
    }

    [Test, MoqAutoData]
    public void Then_The_ClosingDate_More_than_31Days_Is_Shown_Correctly(
        string mapsId,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2000, 01, 01));

        var dateMoreThan31Days = new DateTime(2000, 04, 01);
        if (result.Vacancy != null) result.Vacancy.ClosingDate = dateMoreThan31Days;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result, mapsId);

        Assert.That(actual.ClosingDate, Is.Not.Null);
        actual.ClosingDate.Should().Contain("Closes on");
    }
    
    [Test]
    [MoqInlineAutoData("https://some.url","https://some.url")]
    [MoqInlineAutoData("http://some.url","http://some.url")]
    [MoqInlineAutoData("www.some.url","https://www.some.url")]
    public void Then_If_External_The_Application_Url_Is_Correctly_Formatted_And_Closing_Date(
        string url,
        string expectedUrl,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow.AddDays(-30));
        result.Vacancy.ClosingDate = DateTime.UtcNow.AddDays(-20);
        result.Vacancy.ApplicationUrl = url;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result, "");

        actual.ApplicationUrl.Should().Be(expectedUrl);
        actual.ClosingDate.Should().Be($"Closes in 10 days ({result.Vacancy.ClosingDate:dddd d MMMM})");
    }
    [Test]
    [MoqInlineAutoData("","")]
    [MoqInlineAutoData(null,null)]
    public void Then_If_External_The_Application_Url_Is_Correctly_Formatted_And_Closing_Date_For_Internal(
        string url,
        string expectedUrl,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow.AddDays(-30));
        result.Vacancy.ClosingDate = DateTime.UtcNow.AddDays(-20);
        result.Vacancy.ApplicationUrl = url;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result, "");

        actual.ApplicationUrl.Should().Be(expectedUrl);
        actual.ClosingDate.Should().Be($"Closes in 10 days ({result.Vacancy.ClosingDate:dddd d MMMM} at 11:59pm)");
    }

    [Test]
    [MoqInlineAutoData(AvailableWhere.MultipleLocations)]
    [MoqInlineAutoData(AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData(AvailableWhere.OneLocation)]
    public void Then_If_EmploymentLocation_Multiple_Locations_Is_Returned_As_Expected(
        AvailableWhere availableWhere,
        string expectedResult,
        GetApprenticeshipVacancyQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.Vacancy.EmployerLocationOption = availableWhere;

        var actual = new VacancyDetailsViewModel().MapToViewModel(dateTimeService.Object, result, "");

        actual.EmployerLocationOption.Should().Be(availableWhere);

        switch (availableWhere)
        {
            case AvailableWhere.OneLocation:
                actual.EmploymentWorkLocation.Should().Be(VacancyDetailsHelperService.GetOneLocationCityName(result.Vacancy?.Address));
                break;
            case AvailableWhere.AcrossEngland:
                actual.EmploymentWorkLocation.Should().Be("Recruiting nationally");
                break;
            case AvailableWhere.MultipleLocations:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(availableWhere), availableWhere, null);
        }
        
    }
}