using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
public class WhenCreatingVacancyAdvertsViewModel
{
    [Test, MoqAutoData]
    public void Then_The_Fields_Are_Mapped(VacancyAdvert vacancyAdvert, [Frozen] Mock <IDateTimeService> dateTimeService)
    {
        vacancyAdvert.VacancySource = VacancyDataSource.Raa;

        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);

        actual.Should().BeEquivalentTo(vacancyAdvert, options=> options
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
            .Excluding(c => c.WageAmount)
            .Excluding(c => c.ApprenticeshipLevel)
            .Excluding(c => c.CourseId)
            .Excluding(c => c.CourseLevel)
            .Excluding(c => c.OtherAddresses)
            .Excluding(c => c.AddressLine1)
            .Excluding(c => c.AddressLine2)
            .Excluding(c => c.AddressLine3)
            .Excluding(c => c.AddressLine4)
            .Excluding(c => c.Postcode)
            .Excluding(c => c.EmploymentLocationInformation)
            .Excluding(c => c.StartDate)
            .Excluding(c => c.Over25NationalMinimumWage)
            .Excluding(c => c.Under18NationalMinimumWage)
            .Excluding(c => c.Between18AndUnder21NationalMinimumWage)
            .Excluding(c => c.Between21AndUnder25NationalMinimumWage)
            .Excluding(c => c.ApprenticeMinimumWage)
        );
        actual.CourseTitle.Should().Be($"{vacancyAdvert.CourseTitle} (level {vacancyAdvert.CourseLevel})");
    }
    
    [Test]
    [MoqInlineAutoData("30 Jan 2000")]
    [MoqInlineAutoData("01 Jan 2000")]
    [MoqInlineAutoData("04 Jun 2024")]
    public void Then_The_Closing_Date_Is_Shown_Correctly(
        string closeDate,
        VacancyAdvert vacancyAdvert,
        [Frozen] Mock <IDateTimeService> dateTimeService)
    {
        var closingDate = Convert.ToDateTime(closeDate) ;

        vacancyAdvert.ClosingDate = closingDate;
        vacancyAdvert.ApplicationUrl = null;

        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null); 

        actual.ClosingDateDescription.Should().Be(VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, closingDate, false));
    }

    [Test]
    [MoqInlineAutoData("30 Jan 2000", "Posted 30 January 2000")]
    [MoqInlineAutoData("01 Jan 2000", "Posted 1 January 2000")]
    [MoqInlineAutoData("04 Jun 2024", "Posted 4 June 2024")]
    public void Then_The_Posted_Date_Is_Shown_Correctly(
        string postedDate,
        string expectedResult,
        VacancyAdvert vacancyAdvert,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        vacancyAdvert.PostedDate = Convert.ToDateTime(postedDate);

        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);

        Assert.That(actual.PostedDate, Is.EqualTo(expectedResult));
    }

    [Test, MoqAutoData]
    public void Then_Closing_Soon_Flag_Shown_If_Vacancy_Closes_In_Less_Than_Eight_Days(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var closingDate = new DateTime(2023, 11, 16);
        vacancyAdvert.ApplicationUrl = "";
        dateTimeService.Setup(x => x.GetDateTime()).Returns(closingDate.AddDays(-7));
        
        vacancyAdvert.ClosingDate = closingDate;
        
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);
        actual.IsClosingSoon.Should().BeTrue();
        actual.ClosingDateDescription.Should().Be($"Closes in 7 days ({closingDate:dddd d MMMM yyy} at 11:59pm)");
    }
    
    [Test, MoqAutoData]
    public void Then_Closing_Soon_Flag_Shown_If_Vacancy_Closes_In_Less_Than_Eight_Days_For_External_Application(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var closingDate = new DateTime(2023, 11, 16);
        vacancyAdvert.ApplicationUrl = "someurl";
        dateTimeService.Setup(x => x.GetDateTime()).Returns(closingDate.AddDays(-7));
        
        vacancyAdvert.ClosingDate = closingDate;
        
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);
        actual.IsClosingSoon.Should().BeTrue();
        actual.ClosingDateDescription.Should().Be($"Closes in 7 days ({closingDate:dddd d MMMM yyy})");
    }
    
    [Test, MoqAutoData]
    public void Then_Closing_Soon_Flag_Not_Shown_If_Vacancy_Closes_In_More_Than_Or_Equal_To_Eight_Days(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var closingDate = new DateTime(2023, 11, 16);
        dateTimeService.Setup(x => x.GetDateTime()).Returns(closingDate.AddDays(-8));
        
        vacancyAdvert.ClosingDate = closingDate;
        
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);
        actual.IsClosingSoon.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_New_Flag_Shown_If_Vacancy_Is_Less_Than_Eight_Days_Old(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var postedDate = new DateTime(2023, 11, 16);
        dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2023, 11, 16).AddDays(7));
        
        vacancyAdvert.PostedDate = postedDate;
        
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);
        actual.IsNew.Should().BeTrue();
    }
    
    [Test, MoqAutoData]
    public void Then_New_Flag_Not_Shown_If_Vacancy_Is_Greater_Than_Or_Equal_To_Eight_Days_Old(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var postedDate = new DateTime(2023, 11, 16);
        dateTimeService.Setup(x => x.GetDateTime()).Returns(postedDate.AddDays(8));
        
        vacancyAdvert.PostedDate = postedDate;
        
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);
        actual.IsNew.Should().BeFalse();
    }

    [Test]
    [MoqInlineAutoData("1", "blablaLane","Morden", "London", "EC1", "London (EC1)")]
    [MoqInlineAutoData("1", "blablaLane", "Morden", null, "EC1", "Morden (EC1)")]
    [MoqInlineAutoData("1", "blablaLane",null,null, "EC1", "blablaLane (EC1)")]
    [MoqInlineAutoData("1", null, null, null, "EC1", "1 (EC1)")]
    [MoqInlineAutoData("", null, null, null, "EC1", "EC1")]

    public void Then_The_Address_Is_Shown_Correctly_For_Single_Location_Adverts(
        string addressLine1,
        string? addressLine2,
        string? addressLine3,
        string? addressLine4,
        string postcode,
        string expected,
        VacancyAdvert vacancyAdvert,
        [Frozen] Mock<IDateTimeService> dateTimeService
    )
    {
        vacancyAdvert.OtherAddresses = [];
        vacancyAdvert.AddressLine1 = addressLine1;
        vacancyAdvert.AddressLine2 = addressLine2;
        vacancyAdvert.AddressLine3 = addressLine3;
        vacancyAdvert.AddressLine4 = addressLine4;
        vacancyAdvert.Postcode = postcode;

        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);

        Assert.That(actual.VacancyLocation, Is.EqualTo(expected));
    }
    
    [Test]
    [MoqInlineAutoData("1", "blablaLane","Morden", "London", "EC1", "London (EC1)")]
    [MoqInlineAutoData("1", "blablaLane", "Morden", null, "EC1", "Morden (EC1)")]
    [MoqInlineAutoData("1", "blablaLane",null,null, "EC1", "blablaLane (EC1)")]
    [MoqInlineAutoData("1", null, null, null, "EC1", "1 (EC1)")]
    [MoqInlineAutoData("", null, null, null, "EC1", "EC1")]
    public void Then_The_Address_Is_Shown_Correctly_For_Multiple_Location_Adverts(
        string addressLine1,
        string? addressLine2,
        string? addressLine3,
        string? addressLine4,
        string postcode,
        string expected,
        VacancyAdvert vacancyAdvert,
        [Frozen] Mock<IDateTimeService> dateTimeService
    )
    {
        // arrange
        vacancyAdvert.AddressLine1 = addressLine1;
        vacancyAdvert.AddressLine2 = addressLine2;
        vacancyAdvert.AddressLine3 = addressLine3;
        vacancyAdvert.AddressLine4 = addressLine4;
        vacancyAdvert.Postcode = postcode;

        // act
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);

        // assert
        actual.VacancyLocation.Should().Be($"{expected} and {vacancyAdvert.OtherAddresses!.Count} other locations");
    }

    [Test, MoqAutoData]
    public void The_The_Location_Is_Set_To_Recruiting_Nationally(VacancyAdvert vacancyAdvert, Mock<IDateTimeService> dateTimeService)
    {
        // arrange
        vacancyAdvert.OtherAddresses = [];
        vacancyAdvert.EmploymentLocationInformation = "Some text";
        vacancyAdvert.Postcode = string.Empty;
        
        // act
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);
        
        // assert
        actual.VacancyLocation.Should().Be("Recruiting nationally");
    }

    [Test, MoqAutoData]
    public void Then_The_Distance_Is_Shown_Correctly(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        const decimal distance = 10.897366M;
        const decimal expectedDistance = 10.9M;

        vacancyAdvert.Distance = distance;

        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);

        Assert.That(actual.Distance, Is.EqualTo(expectedDistance));
    }

    [Test, MoqAutoData]
    public void Then_Days_Until_Advert_Closes_Is_Shown_Correctly(VacancyAdvertViewModel viewModel,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        var comparisonDate = new DateTime(2023, 11, 16);
        var closingDate = new DateTime(2023, 11, 30);
        var expected = 14;

        dateTimeService.Setup(x => x.GetDateTime()).Returns(comparisonDate);

        var actual = VacancyAdvertViewModel.CalculateDaysUntilClosing(dateTimeService.Object, closingDate);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test, MoqAutoData]
    public void Then_The_Nhs_Vacancies_Fields_Are_Mapped_Returned_As_Expected(VacancyAdvert vacancyAdvert, [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        vacancyAdvert.VacancySource = VacancyDataSource.Nhs;
        var actual = VacancyAdvertViewModel.MapToViewModel(dateTimeService.Object, vacancyAdvert, null);

        actual.Should().BeEquivalentTo(vacancyAdvert, options => options
            .Excluding(c => c.ClosingDate)
            .Excluding(c => c.PostedDate)
            .Excluding(c => c.Id)
            .Excluding(c => c.CourseTitle)
            .Excluding(c => c.Title)
            .Excluding(c => c.Postcode)
            .Excluding(c => c.CandidateApplicationDetails)
            .Excluding(c => c.ApplicationStatus)
            .Excluding(c => c.Lat)
            .Excluding(c => c.Lon)
            .Excluding(c => c.IsNew)
            .Excluding(c => c.IsClosingSoon)
            .Excluding(c => c.ApplicationUrl)
            .Excluding(c => c.WageType)
            .Excluding(c => c.WageAmount)
            .Excluding(c => c.ApprenticeshipLevel)
            .Excluding(c => c.CourseId)
            .Excluding(c => c.CourseLevel)
            .Excluding(c => c.OtherAddresses)
            .Excluding(c => c.AddressLine1)
            .Excluding(c => c.AddressLine2)
            .Excluding(c => c.AddressLine3)
            .Excluding(c => c.AddressLine4)
            .Excluding(c => c.Postcode)
            .Excluding(c => c.EmploymentLocationInformation)
            .Excluding(c => c.StartDate)
            .Excluding(c => c.Over25NationalMinimumWage)
            .Excluding(c => c.Under18NationalMinimumWage)
            .Excluding(c => c.Between18AndUnder21NationalMinimumWage)
            .Excluding(c => c.Between21AndUnder25NationalMinimumWage)
            .Excluding(c => c.ApprenticeMinimumWage)
        );
        actual.CourseTitle.Should().Be("See more details on NHS Jobs");
        actual.Title.Should().Be($"{vacancyAdvert.Title} (from NHS Jobs)");
    }

}