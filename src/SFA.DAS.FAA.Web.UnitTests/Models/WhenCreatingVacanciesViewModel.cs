using Moq;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
[TestFixture]
public class WhenCreatingVacanciesViewModel
{
    [Test, MoqAutoData]
    public async Task Then_The_Closing_Date_Is_Shown_Correctly(Vacancies vacancies)
    {

        DateTime closingDate = new DateTime(2023, 11, 16) ;
        string expectedClosingDate = "Thursday 16 November";

        vacancies.ClosingDate = closingDate;
        var source = vacancies;

        var actual = (VacanciesViewModel)source;

        Assert.AreEqual(expectedClosingDate, actual.AdvertClosing);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Posted_Date_Is_Shown_Correctly(Vacancies vacancies)
    {

        DateTime postedDate = new DateTime(2023, 11, 16);
        string expectedPostedDate = "16 November";

        vacancies.PostedDate = postedDate;
        var source = vacancies;

        var actual = (VacanciesViewModel)source;

        Assert.AreEqual(expectedPostedDate, actual.PostedDate);
    }

    [Theory]
    [InlineAutoData("1","blablaLane","Morden", "London", "London")]
    [InlineAutoData("1", "blablaLane", "Morden", null, "Morden")]
    [InlineAutoData("1", "blablaLane",null,null, "blablaLane")]
    [InlineAutoData("1", null, null, null, "1")]

    public void Then_The_Address_Is_Shown_Correctly(string addressLine1, string? addressLine2, string? addressLine3, string? addressLine4, string expected,
        [Frozen] Vacancies vacancies
    )
    {
        vacancies.Address.AddressLine1 = addressLine1;
        vacancies.Address.AddressLine2 = addressLine2;
        vacancies.Address.AddressLine3 = addressLine3;
        vacancies.Address.AddressLine4 = addressLine4;

        var source = vacancies;
        var actual = (VacanciesViewModel)source;


        Assert.AreEqual(expected, actual.VacancyLocation);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Distance_Is_Shown_Correctly(Vacancies vacancies)
    {

        double distance = 10.87546346;
        double expectedDistance = 10.9;

        vacancies.Distance = distance;
        var source = vacancies;

        var actual = (VacanciesViewModel)source;

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

    [Theory]
    [InlineAutoData(4, (double)20400, "£20,400.00")]
    [InlineAutoData(2, null, "£10,158.72")]
    [InlineAutoData(3, null, "£10,982.40 to £21,673.60")]

    public void Then_The_wage_Is_Shown_Correctly(int wageType,string wageAmount, string expected,
        [Frozen] Vacancies vacancies
    )
    {
        vacancies.WageType = wageType;
        vacancies.WageAmount = wageAmount;


        var source = vacancies;
        var actual = (VacanciesViewModel)source;

        Assert.AreEqual(expected, actual.Wage);

    }
}