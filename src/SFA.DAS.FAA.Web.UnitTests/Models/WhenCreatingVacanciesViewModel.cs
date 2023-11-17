using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Globalization;
using System.Net;
using AutoFixture.NUnit3;
using Xunit;
using TheoryAttribute = NUnit.Framework.TheoryAttribute;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        vacancies.closingDate = closingDate;
        var source = vacancies;

        var actual = (VacanciesViewModel)source;

        Assert.AreEqual(expectedClosingDate, actual.advertClosing);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Posted_Date_Is_Shown_Correctly(Vacancies vacancies)
    {

        DateTime postedDate = new DateTime(2023, 11, 16);
        string expectedPostedDate = "16 November";

        vacancies.postedDate = postedDate;
        var source = vacancies;

        var actual = (VacanciesViewModel)source;

        Assert.AreEqual(expectedPostedDate, actual.postedDate);
    }

    [Theory]
    [MoqInlineAutoData("1","blablaLane","Morden", "London", "London")]
    [MoqInlineAutoData("1", "blablaLane", "Morden", null, "Morden")]
    [MoqInlineAutoData("1", "blablaLane",null,null, "blablaLane")]
    [MoqInlineAutoData("1", null, null, null, "1")]

    public void Then_The_Address_Is_Shown_Correctly(string addressLine1, string? addressLine2, string? addressLine3, string? addressLine4, string expected,
        [Frozen] Vacancies vacancies
    )
    {
        vacancies.address.addressLine1 = addressLine1;
        vacancies.address.addressLine2 = addressLine2;
        vacancies.address.addressLine3 = addressLine3;
        vacancies.address.addressLine4 = addressLine4;

        var source = vacancies;
        var actual = (VacanciesViewModel)source;


        Assert.AreEqual(expected, actual.placeName);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Distance_Is_Shown_Correctly(Vacancies vacancies)
    {

        double distance = 10.87546346;
        double expectedDistance = 10.9;

        vacancies.distance = distance;
        var source = vacancies;

        var actual = (VacanciesViewModel)source;

        Assert.AreEqual(expectedDistance, actual.distance);
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
    [MoqInlineAutoData("Custom", (double)20400, "£20,400.00")]
    [MoqInlineAutoData("ApprenticeshipMinimum", null, "£10,158.72")]
    [MoqInlineAutoData("NationalMinimum", null, "£10,982.40 to £21,673.60")]

    public void Then_The_wage_Is_Shown_Correctly(string wageType,double? wageAmount, string expected,
        [Frozen] Vacancies vacancies
    )
    {
        vacancies.wage.wageType = wageType;
        vacancies.wage.wageAmount = wageAmount;


        var source = vacancies;
        var actual = (VacanciesViewModel)source;

            Assert.AreEqual(expected, actual.wage);

    }
}