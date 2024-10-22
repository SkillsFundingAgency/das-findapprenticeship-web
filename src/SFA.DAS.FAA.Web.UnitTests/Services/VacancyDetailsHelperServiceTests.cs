using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    public class VacancyDetailsHelperServiceTests
    {
        [TestCase("", "")]
        [TestCase(null, null)]
        [TestCase("www.someUrl.com", "http://www.someUrl.com")]
        [TestCase("http://www.someUrl.com", "http://www.someUrl.com")]
        [TestCase("https://www.someUrl.com", "https://www.someUrl.com")]
        public void FormatEmployerWebsiteUrl(string url, string? expectedUrl)
        {
            //sut
            var result = VacancyDetailsHelperService.FormatEmployerWebsiteUrl(url);

            //assert
            result.Should().Be(expectedUrl);
        }

        [TestCase("", "0 hours a week")]
        [TestCase(null, "0 hours a week")]
        [TestCase("37", "37 hours a week")]
        [TestCase("37.5", "37 hours 30 minutes a week")]
        public void GetWorkingHours(string hours, string? expectedResult)
        {
            //sut
            var result = hours.GetWorkingHours();

            //assert
            result.Should().Be(expectedResult);
        }

        [TestCase("30 Jan 2000", "Closes in 29 days (Sunday 30 January at 11:59pm)")]
        [TestCase("01 Feb 2000", "Closes in 31 days (Tuesday 1 February at 11:59pm)")]
        [TestCase("01 Jan 2000", "Closes today at 11:59pm")]
        [TestCase("02 Jan 2000", "Closes tomorrow (Sunday 2 January at 11:59pm)")]
        [TestCase("01 Apr 2000", "Closes on Saturday 1 April")]
        public void GetClosingDate(string closingDate, string? expectedResult)
        {
            //arrange
            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(date => date.GetDateTime()).Returns(new DateTime(2000, 01, 01));

            //sut
            var result =
                VacancyDetailsHelperService.GetClosingDate(mockDateTimeService.Object, Convert.ToDateTime(closingDate));

            //assert
            result.Should().Be(expectedResult);
        }

        [TestCase("30 Jan 2000", "Posted on 30 January 2000")]
        [TestCase("01 Jan 2000", "Posted on 1 January 2000")]
        [TestCase("04 Jun 2024", "Posted on 4 June 2024")]
        public void GetPostedDate(string postedDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(postedDate).GetPostedDate();

            //assert
            result.Should().Be(expectedResult);
        }

        [TestCase("30 Jan 2000", "Sunday 30 January")]
        [TestCase("01 Jan 2000", "Saturday 1 January")]
        [TestCase("04 Jun 2024", "Tuesday 4 June")]
        public void GetStartDate(string startDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(startDate).GetStartDate();

            //assert
            result.Should().Be(expectedResult);
        }

        [TestCase("30 Jan 2000", "Posted on 30 January")]
        [TestCase("01 Jan 2000", "Posted on 1 January")]
        [TestCase("04 Jun 2024", "Posted on 4 June")]
        public void GetMapsPostedDate(string startDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(startDate).GetMapsPostedDate();

            //assert
            result.Should().Be(expectedResult);
        }

        [Test]
        [InlineAutoData(-1, 20, "Closed on")]
        [InlineAutoData(0, 3, "Closed on")]
        public void GetClosingDate_Overload_Checks_ClosedDate_Before_ClosingDate(int closedDays, int closingDays, string expected, Mock<IDateTimeService> dateTimeService)
        {
            // arrange
            dateTimeService.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);
            var closedWhen = DateTime.UtcNow.AddDays(closedDays);
            var closingWhen = DateTime.UtcNow.AddDays(closingDays);
            
            // act
            var actual = VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, closingWhen, closedWhen);

            // assert
            actual.Should().StartWith("Closed on");
        }
        
        [Test]
        [InlineAutoData("30 Jan 2000", "Closes in 29 days (Sunday 30 January at 11:59pm)")]
        [InlineAutoData("01 Feb 2000", "Closes in 31 days (Tuesday 1 February at 11:59pm)")]
        [InlineAutoData("01 Jan 2000", "Closes today at 11:59pm")]
        [InlineAutoData("02 Jan 2000", "Closes tomorrow (Sunday 2 January at 11:59pm)")]
        [InlineAutoData("01 Apr 2000", "Closes on Saturday 1 April")]
        public void GetClosingDate_Overload_Has_The_Default_Behaviour_When_ClosedDate_Is_Null(string closingDate, string expected, Mock<IDateTimeService> dateTimeService)
        {
            // arrange
            dateTimeService.Setup(x => x.GetDateTime()).Returns(new DateTime(2000, 01, 01));
            var closingWhen = Convert.ToDateTime(closingDate);
            
            // act
            var actual = VacancyDetailsHelperService.GetClosingDate(dateTimeService.Object, closingWhen, null);

            // assert
            actual.Should().Be(expected);
        }
    }
}