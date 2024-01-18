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

        [TestCase("30 Jan 2000", "Closes in 29 days (Sunday 30 January at 11.59pm)")]
        [TestCase("01 Feb 2000", "Closes on Tuesday 01 February")]
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
        [TestCase("01 Jan 2000", "Posted on 01 January 2000")]
        public void GetPostedDate(string postedDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(postedDate).GetPostedDate();

            //assert
            result.Should().Be(expectedResult);
        }

        [TestCase("30 Jan 2000", "Sunday 30 January")]
        [TestCase("01 Jan 2000", "Saturday 01 January")]
        public void GetStartDate(string startDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(startDate).GetStartDate();

            //assert
            result.Should().Be(expectedResult);
        }
    }
}