using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Services
{
    [TestFixture]
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

        [TestCase("30 Jan 2000", "Closes in 29 days (Sunday 30 January 2000 at 11:59pm)")]
        [TestCase("01 Feb 2000", "Closes in 31 days (Tuesday 1 February 2000 at 11:59pm)")]
        [TestCase("01 Jan 2000", "Closes today at 11:59pm")]
        [TestCase("02 Jan 2000", "Closes tomorrow (Sunday 2 January 2000 at 11:59pm)")]
        [TestCase("01 Apr 2000", "Closes on Saturday 1 April 2000")]
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

        [TestCase("30 Jan 2000", "Posted on 30 January 2000")]
        [TestCase("01 Jan 2000", "Posted on 1 January 2000")]
        [TestCase("04 Jun 2024", "Posted on 4 June 2024")]
        public void GetMapsPostedDate(string startDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(startDate).GetMapsPostedDate();

            //assert
            result.Should().Be(expectedResult);
        }

        [Test]
        [InlineAutoData(-1, 20)]
        [InlineAutoData(0, 3)]
        public void GetClosingDate_Overload_Checks_ClosedDate_Before_ClosingDate(int closedDays, int closingDays, Mock<IDateTimeService> dateTimeService)
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
        [InlineAutoData("30 Jan 2000", "Closes in 29 days (Sunday 30 January 2000 at 11:59pm)")]
        [InlineAutoData("01 Feb 2000", "Closes in 31 days (Tuesday 1 February 2000 at 11:59pm)")]
        [InlineAutoData("01 Jan 2000", "Closes today at 11:59pm")]
        [InlineAutoData("02 Jan 2000", "Closes tomorrow (Sunday 2 January 2000 at 11:59pm)")]
        [InlineAutoData("01 Apr 2000", "Closes on Saturday 1 April 2000")]
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

        
        [Test]
        [InlineAutoData(null, "")] // Null input
        [InlineAutoData("", "")] // Empty input
        public void GetWageText_ShouldHandleNullOrEmptyInput(string input, string expected)
        {
            // Act
            var result = VacancyDetailsHelperService.GetWageText(input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("£50", "£50 an hour")] // Less than 100
        [InlineAutoData("£50.00", "£50 an hour")] // Less than 100
        [InlineAutoData("£99.99", "£99.99 an hour")] // Edge case: Just below 100
        public void GetWageText_ShouldHandleHourlyWages(string input, string expected)
        {
            // Act
            var result = VacancyDetailsHelperService.GetWageText(input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("£5001.00", "£5,001 a year")] // Greater than 5000
        [InlineAutoData("£6000.75", "£6,000.75 a year")] // Above threshold
        public void GetWageText_ShouldHandleAnnualWages(string input, string expected)
        {
            // Act
            var result = VacancyDetailsHelperService.GetWageText(input);

            // Assert
            result.Should().Be(expected);
        }
        
        [Test]
        [InlineAutoData("£4000.75 to £5000.25", "£4,000.75 to £5,000.25 a year")] 
        [InlineAutoData("£4000.00 to £5001.00", "£4,000 to £5,001 a year")] 
        [InlineAutoData("£4000.00 to £5000.00", "£4,000 to £5,000")] 
        [InlineAutoData("£5001.00 to £15000.00", "£5,001 to £15,000 a year")] // Greater than 5000
        [InlineAutoData("£1000.00 to £4999.00", "£1,000 to £4,999")] // Below threshold
        [InlineAutoData("£8.00 to £15.00", "£8 to £15 an hour")] // Below lower limit
        [InlineAutoData("£8.50 to £11.10", "£8.50 to £11.10 an hour")] // Below lower limit
        public void Then_WageAmount_With_Lower_Higher_Limit_GetWageText_ShouldHandleAnnualWages(string input, string expected)
        {
            // Act
            var result = VacancyDetailsHelperService.GetWageText(input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("£100", "£100")] // Between 100 and 5000
        [InlineAutoData("£4999.99", "£4999.99")] // Edge case: Just below 5000
        [InlineAutoData("NoPoundSign", "NoPoundSign")] // Invalid format
        [InlineAutoData("£Invalid", "£Invalid")] // Invalid numeric value
        public void GetWageText_ShouldReturnOriginalTextForOtherCases(string input, string expected)
        {
            // Act
            var result = VacancyDetailsHelperService.GetWageText(input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void GetEmploymentLocationCityNames_ShouldReturnSingleCityWithCount_WhenMultipleAddressesInSameCity()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new("123 Main St", "Suite 1", "CityA", "Region", "12345"),
                new("456 Main St", "Suite 2", "CityA", "Region", "12345"),
            };

            // Act
            var result = VacancyDetailsHelperService.GetEmploymentLocationCityNames(addresses);

            // Assert
            result.Should().Be("Region (2 available locations)");
        }

        [Test]
        public void GetEmploymentLocationCityNames_ShouldReturnCommaSeparatedCities_WhenMultipleAddressesInDifferentCities()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new("123 Main St", "Suite 1", "CityA", "Region", "12345"),
                new("456 Main St", "Suite 2", "CityB", "Region", "12345"),
            };

            // Act
            var result = VacancyDetailsHelperService.GetEmploymentLocationCityNames(addresses);

            // Assert
            result.Should().Be("Region (2 available locations)");
        }

        [Test]
        public void GetEmploymentLocationCityNames_ShouldReturnEmptyString_WhenNoAddressesProvided()
        {
            // Arrange
            var addresses = new List<Address>();

            // Act
            var result = VacancyDetailsHelperService.GetEmploymentLocationCityNames(addresses);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnCityWithPostcode_WhenCityIsNotEmpty()
        {
            // Arrange
            var address = new Address("Line1", "Line2", "Line3", "City", "12345");

            // Act
            var result = VacancyDetailsHelperService.GetOneLocationCityName(address);

            // Assert
            result.Should().Be("City (12345)");
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnPostcode_WhenCityIsEmpty()
        {
            // Arrange
            var address = new Address(null, null, null, null, "12345");

            // Act
            var result = VacancyDetailsHelperService.GetOneLocationCityName(address);

            // Assert
            result.Should().Be("12345");
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnFirstNonEmptyAddressLineWithPostcode_WhenCityIsEmpty()
        {
            // Arrange
            var address = new Address("Line1", "Line2", "Line3", null, "12345");

            // Act
            var result = VacancyDetailsHelperService.GetOneLocationCityName(address);

            // Assert
            result.Should().Be("Line3 (12345)");
        }

        [Test]
        [MoqInlineAutoData("5-Oct-2023", "5 October 2023")]
        [MoqInlineAutoData("05-Oct-2023", "5 October 2023")]
        [MoqInlineAutoData("10-Oct-2023", "10 October 2023")]
        [MoqInlineAutoData("0001-01-01", "1 January 0001")]
        public void ToFullDateString_ShouldReturnFormattedDate(string startDate, string expected)
        {
            // Arrange
            var date = Convert.ToDateTime(startDate);

            // Act
            var result = date.ToFullDateString();

            // Assert
            result.Should().Be(expected);
        }
    }
}