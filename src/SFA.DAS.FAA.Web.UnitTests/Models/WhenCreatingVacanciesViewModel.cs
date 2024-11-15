using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
public class WhenCreatingVacanciesViewModel
{
    [Test, MoqAutoData]
    public void Then_The_Fields_Are_Mapped(Vacancies vacancies, [Frozen] Mock <IDateTimeService> dateTimeService)
    {
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);

        actual.Should().BeEquivalentTo(vacancies, options=> options
            .Excluding(c=>c.ClosingDate)
            .Excluding(c=>c.PostedDate)
            .Excluding(c=>c.Id)
            .Excluding(c=>c.CourseTitle)
            .Excluding(c=>c.Postcode)
            .Excluding(c => c.CandidateApplicationDetails)
            .Excluding(c => c.ApplicationStatus)
            .Excluding(c => c.Lat)
            .Excluding(c => c.Lon)
            .Excluding(c => c.IsNew)
            .Excluding(c => c.IsClosingSoon)
            .Excluding(c => c.ApplicationUrl)
            .Excluding(c => c.WageType)
        );
        actual.CourseTitle.Should().Be($"{vacancies.CourseTitle} (level {vacancies.CourseLevel})");
        actual.VacancyPostCode.Should().Be(vacancies.Postcode);
    }
    
    [Test]
    [MoqInlineAutoData("30 Jan 2000")]
    [MoqInlineAutoData("01 Jan 2000")]
    [MoqInlineAutoData("04 Jun 2024")]
    public void Then_The_Closing_Date_Is_Shown_Correctly(
        string closeDate,
        Vacancies vacancies,
        [Frozen] Mock <IDateTimeService> dateTimeService)
    {
        var closingDate = Convert.ToDateTime(closeDate) ;

        vacancies.ClosingDate = closingDate;
        vacancies.ApplicationUrl = null;

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies); 

        actual.ClosingDateDescription.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, closingDate, false));
    }

    [Test]
    [MoqInlineAutoData("30 Jan 2000", "30 January")]
    [MoqInlineAutoData("01 Jan 2000", "1 January")]
    [MoqInlineAutoData("04 Jun 2024", "4 June")]
    public void Then_The_Posted_Date_Is_Shown_Correctly(
        string postedDate,
        string expectedResult,
        Vacancies vacancies,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        vacancies.PostedDate = Convert.ToDateTime(postedDate);

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);

        Assert.That(actual.PostedDate, Is.EqualTo(expectedResult));
    }

    [Test, MoqAutoData]
    public void Then_Closing_Soon_Flag_Shown_If_Vacancy_Closes_In_Less_Than_Eight_Days(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var closingDate = new DateTime(2023, 11, 16);
        vacancies.ApplicationUrl = "";
        dateTimeService.Setup(x => x.GetDateTime()).Returns(closingDate.AddDays(-7));
        
        vacancies.ClosingDate = closingDate;
        
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);
        actual.IsClosingSoon.Should().BeTrue();
        actual.ClosingDateDescription.Should().Be($"Closes in 7 days ({closingDate:dddd d MMMM} at 11:59pm)");
    }
    
    [Test, MoqAutoData]
    public void Then_Closing_Soon_Flag_Shown_If_Vacancy_Closes_In_Less_Than_Eight_Days_For_External_Application(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var closingDate = new DateTime(2023, 11, 16);
        vacancies.ApplicationUrl = "someurl";
        dateTimeService.Setup(x => x.GetDateTime()).Returns(closingDate.AddDays(-7));
        
        vacancies.ClosingDate = closingDate;
        
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);
        actual.IsClosingSoon.Should().BeTrue();
        actual.ClosingDateDescription.Should().Be($"Closes in 7 days ({closingDate:dddd d MMMM})");
    }
    
    [Test, MoqAutoData]
    public void Then_Closing_Soon_Flag_Not_Shown_If_Vacancy_Closes_In_More_Than_Or_Equal_To_Eight_Days(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var closingDate = new DateTime(2023, 11, 16);
        dateTimeService.Setup(x => x.GetDateTime()).Returns(closingDate.AddDays(-8));
        
        vacancies.ClosingDate = closingDate;
        
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);
        actual.IsClosingSoon.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_New_Flag_Shown_If_Vacancy_Is_Less_Than_Eight_Days_Old(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var postedDate = new DateTime(2023, 11, 16);
        dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2023, 11, 16).AddDays(7));
        
        vacancies.PostedDate = postedDate;
        
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);
        actual.IsNew.Should().BeTrue();
    }
    
    [Test, MoqAutoData]
    public void Then_New_Flag_Not_Shown_If_Vacancy_Is_Greater_Than_Or_Equal_To_Eight_Days_Old(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var postedDate = new DateTime(2023, 11, 16);
        dateTimeService.Setup(x => x.GetDateTime()).Returns(postedDate.AddDays(8));
        
        vacancies.PostedDate = postedDate;
        
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);
        actual.IsNew.Should().BeFalse();
    }

    [Test]
    [MoqInlineAutoData("1","blablaLane","Morden", "London", "EC1", "London, EC1")]
    [MoqInlineAutoData("1", "blablaLane", "Morden", null, "EC1", "Morden, EC1")]
    [MoqInlineAutoData("1", "blablaLane",null,null, "EC1", "blablaLane, EC1")]
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

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);

        Assert.That(actual.VacancyLocation, Is.EqualTo(expected));
    }

    [Test, MoqAutoData]
    public void Then_The_Distance_Is_Shown_Correctly(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {

        var distance = 10.897366M;
        var expectedDistance = 10.9M;

        vacancies.Distance = distance;

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);

        Assert.That(actual.Distance, Is.EqualTo(expectedDistance));
    }

    [Test, MoqAutoData]
    public void Then_Days_Until_Advert_Closes_Is_Shown_Correctly(VacanciesViewModel viewModel,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var comparisonDate = new DateTime(2023, 11, 16);
        var closingDate = new DateTime(2023, 11, 30);
        var expected = 14;

        dateTimeService.Setup(x => x.GetDateTime()).Returns(comparisonDate);

        var actual = VacanciesViewModel.CalculateDaysUntilClosing(dateTimeService.Object, closingDate);

        Assert.That(actual, Is.EqualTo(expected));
    }

}