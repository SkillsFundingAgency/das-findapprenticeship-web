using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenMappingVacanciesToMapData
{
    [Test, MoqAutoData]
    public void Then_The_Fields_Are_Mapped(
        Vacancies source, 
        [Frozen]Mock<IDateTimeService> dateTimeService)
    {
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);
        
        var actual = new ApprenticeshipMapData().MapToViewModel(dateTimeService.Object, source);

        actual.Position.Lat.Should().Be(source.Lat);
        actual.Position.Lng.Should().Be(source.Lon);
        actual.Job.Title.Should().Be(source.Title);
        actual.Job.Apprenticeship.Should().Be($"{source.CourseTitle} (level {source.CourseLevel})");
        actual.Job.Company.Should().Be(source.EmployerName);
        actual.Job.Wage.Should().Be(source.WageText);
        actual.Job.ClosingDate.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, source.ClosingDate));
    }

    [Test]
    [MoqInlineAutoData("1", "blablaLane", "Morden", "London", "EC1", "London, EC1")]
    [MoqInlineAutoData("1", "blablaLane", "Morden", null, "EC1", "Morden, EC1")]
    [MoqInlineAutoData("1", "blablaLane", null, null, "EC1", "blablaLane, EC1")]
    [MoqInlineAutoData("1", null, null, null, "EC1", "1, EC1")]
    [MoqInlineAutoData("", null, null, null, "EC1", "EC1")]

    public void Then_The_Address_Is_Shown_Correctly(string addressLine1, string? addressLine2, string? addressLine3, string? addressLine4, string postcode, string expected,
        Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService
    )
    {
        vacancies.AddressLine1 = addressLine1;
        vacancies.AddressLine2 = addressLine2;
        vacancies.AddressLine3 = addressLine3;
        vacancies.AddressLine4 = addressLine4;
        vacancies.Postcode = postcode;

        var actual = new ApprenticeshipMapData().MapToViewModel(dateTimeService.Object, vacancies);

        Assert.That(actual.Job.VacancyLocation, Is.EqualTo(expected));
    }
}