using Moq;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Services;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
public class WhenCreatingVacanciesViewModel
{
    [Test, MoqAutoData]
    public async Task Then_The_Closing_Date_Is_Shown_Correctly(Vacancies vacancies, [Frozen] Mock <IDateTimeService> dateTimeService)
    {

        DateTime closingDate = new DateTime(2023, 11, 16) ;
        string expectedClosingDate = "Thursday 16 November";

        vacancies.ClosingDate = closingDate;
        var source = vacancies;

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies); 

        Assert.AreEqual(expectedClosingDate, actual.AdvertClosing);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Posted_Date_Is_Shown_Correctly(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {

        DateTime postedDate = new DateTime(2023, 11, 16);
        string expectedPostedDate = "16 November";

        vacancies.PostedDate = postedDate;
        var source = vacancies;

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);

        Assert.AreEqual(expectedPostedDate, actual.PostedDate);
    }

    [Test]
    [MoqInlineAutoData("1","blablaLane","Morden", "London", "London")]
    [MoqInlineAutoData("1", "blablaLane", "Morden", null, "Morden")]
    [MoqInlineAutoData("1", "blablaLane",null,null, "blablaLane")]
    [MoqInlineAutoData("1", null, null, null, "1")]

    public void Then_The_Address_Is_Shown_Correctly(string addressLine1, string? addressLine2, string? addressLine3, string? addressLine4, string expected,
        Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService
    )
    {
        vacancies.AddressLine1 = addressLine1;
        vacancies.AddressLine2 = addressLine2;
        vacancies.AddressLine3 = addressLine3;
        vacancies.AddressLine4 = addressLine4;

        var source = vacancies;
        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);


        Assert.AreEqual(expected, actual.VacancyLocation);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Distance_Is_Shown_Correctly(Vacancies vacancies, [Frozen] Mock<IDateTimeService> dateTimeService)
    {

        decimal distance = 10.897366M;
        decimal expectedDistance = 10.9M;

        vacancies.Distance = distance;
        var source = vacancies;

        var actual = new VacanciesViewModel().MapToViewModel(dateTimeService.Object, vacancies);

        Assert.AreEqual(expectedDistance, actual.Distance);
    }

    [Test, MoqAutoData]
    public async Task Then_Days_Until_Advert_Closes_Is_Shown_Correctly(VacanciesViewModel viewModel,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        DateTime comparisonDate = new DateTime(2023, 11, 16);
        DateTime? closingDate = new DateTime(2023, 11, 30);
        int? expected = 14;

        dateTimeService.Setup(x => x.GetDateTime()).Returns(comparisonDate);

        var actual = VacanciesViewModel.CalculateDaysUntilClosing(dateTimeService.Object, closingDate);

        Assert.AreEqual(expected, actual);
    }

}