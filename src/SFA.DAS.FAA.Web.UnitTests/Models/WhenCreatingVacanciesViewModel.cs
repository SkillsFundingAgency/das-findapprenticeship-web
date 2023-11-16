using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Globalization;

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

    [Test]
    [TestCase("1","blablaLane","Morden", "London", "London")]
    [TestCase("1", "blablaLane", "Morden", null, "Morden")]
    [TestCase("1", "blablaLane",null,null, "blablaLane")]
    [TestCase("1", null, null, null, "1")]

    public async Task Then_The_Address_Is_Shown_Correctly(string addressLine1, string? addressLine2, string? addressLine3, string? AddressLine4, string expected)
    {
        var vacanciesMock = new Mock<Vacancies>();
        vacanciesMock.Setup(v => v.address.addressLine1).Returns(addressLine1);
        vacanciesMock.Setup(v => v.address.addressLine2).Returns(addressLine2);
        vacanciesMock.Setup(v => v.address.addressLine3).Returns(addressLine3);
        vacanciesMock.Setup(v => v.address.addressLine4).Returns(AddressLine4);

        var actual = (VacanciesViewModel)vacanciesMock.Object;

        Assert.AreEqual(expected, actual.placeName);
    }

}