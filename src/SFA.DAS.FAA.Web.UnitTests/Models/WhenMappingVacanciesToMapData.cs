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
        VacancyAdvert source, 
        [Frozen]Mock<IDateTimeService> dateTimeService)
    {
        source.ApplicationUrl = string.Empty;
        dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);
        
        var actual = ApprenticeshipMapData.MapToViewModel(dateTimeService.Object, source);

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
        VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService
    )
    {
        vacancyAdvert.AddressLine1 = addressLine1;
        vacancyAdvert.AddressLine2 = addressLine2;
        vacancyAdvert.AddressLine3 = addressLine3;
        vacancyAdvert.AddressLine4 = addressLine4;
        vacancyAdvert.Postcode = postcode;

        var actual = ApprenticeshipMapData.MapToViewModel(dateTimeService.Object, vacancyAdvert);

        Assert.That(actual.Job.VacancyLocation, Is.EqualTo(expected));
    }
}