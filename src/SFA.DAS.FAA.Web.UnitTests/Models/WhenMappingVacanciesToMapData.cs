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
}