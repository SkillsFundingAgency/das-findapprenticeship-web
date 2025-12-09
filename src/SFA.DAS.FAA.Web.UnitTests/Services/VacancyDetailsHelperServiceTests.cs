using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Extensions;
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

        [TestCase("30 Jan 2000", "Posted 30 January 2000")]
        [TestCase("01 Jan 2000", "Posted 1 January 2000")]
        [TestCase("04 Jun 2024", "Posted 4 June 2024")]
        public void GetSearchResultsPostedDate(string postedDate, string? expectedResult)
        {
            //sut
            var result = Convert.ToDateTime(postedDate).GetSearchResultsPostedDate();

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
            var result = VacancyDetailsHelperService.GetExternalVacancyAdvertWageText(input);

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
            var result = VacancyDetailsHelperService.GetExternalVacancyAdvertWageText(input);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        [InlineAutoData("£5001.00", "£5,001 a year")] // Greater than 5000
        [InlineAutoData("£6000.75", "£6,000.75 a year")] // Above threshold
        public void GetWageText_ShouldHandleAnnualWages(string input, string expected)
        {
            // Act
            var result = VacancyDetailsHelperService.GetExternalVacancyAdvertWageText(input);

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
            var result = VacancyDetailsHelperService.GetExternalVacancyAdvertWageText(input);

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
            var result = VacancyDetailsHelperService.GetExternalVacancyAdvertWageText(input);

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

        [Test]
        [InlineAutoData(WageType.Unknown)]
        [InlineAutoData(WageType.FixedWage)]
        [InlineAutoData(WageType.CompetitiveSalary)]
        [InlineAutoData(WageType.NationalMinimumWage)]
        public void GetVacancyAdvertWageText_Returns_Wage_Text_When_No_Candidate_Date_Of_Birth(WageType wageType, VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)wageType;
            
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert);
            
            actual.Should().Be(vacancyAdvert.WageText);
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_Under_18_Candidates_For_MinimumWageType(VacancyAdvert vacancyAdvert)
        {
            
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
            vacancyAdvert.StartDate = DateTime.Now.AddYears(1);
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, DateTime.Now.AddYears(-16));
            
            actual.Should().Be(vacancyAdvert.Under18NationalMinimumWage.ToDisplayWage());
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_18_to_Under_21_Candidate_For_MinimumWageType(VacancyAdvert vacancyAdvert)
        {
            
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
            vacancyAdvert.StartDate = DateTime.Now.AddYears(1);
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, DateTime.Now.AddYears(-19));
            
            actual.Should().Be(vacancyAdvert.Between18AndUnder21NationalMinimumWage.ToDisplayWage());
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_21_to_Under_25_Candidate_For_MinimumWageType(VacancyAdvert vacancyAdvert)
        {
            
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
            vacancyAdvert.StartDate = DateTime.Now.AddYears(1);
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, DateTime.Now.AddYears(-23));
            
            actual.Should().Be(vacancyAdvert.Between21AndUnder25NationalMinimumWage.ToDisplayWage());
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_Over_25_Candidate_For_MinimumWageType(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
            vacancyAdvert.StartDate = DateTime.Now.AddYears(1);
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, DateTime.Now.AddYears(-25));
            
            actual.Should().Be(vacancyAdvert.Over25NationalMinimumWage.ToDisplayWage());
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_Candidates_Date_Of_Birth_And_Advert_Start_Date_For_ApprenticeMinimumWageType(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWageForApprentices;
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, new DateTime());
            
            actual.Should().Be(vacancyAdvert.ApprenticeMinimumWage.ToDisplayWage());
            
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Empty_String_If_No_Wage_Text(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageText = null;
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, null, true);
            
            actual.Should().BeNull();
            
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_Candidates_Date_Of_Birth_And_Advert_Start_Date_For_ApprenticeMinimumWageType_For_Detail(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWageForApprentices;
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, new DateTime(), true);
            
            actual.Should().Be(vacancyAdvert.ApprenticeMinimumWage.ToDisplayWage("for your first year, then could increase depending on your age"));
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_No_Date_Of_Birth_And_Advert_Start_Date_For_ApprenticeMinimumWageType_For_Detail(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWageForApprentices;
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, null, true);
            
            actual.Should().Be(vacancyAdvert.ApprenticeMinimumWage.ToDisplayWage("for your first year, then could increase depending on your age"));
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_No_Date_Of_Birth_And_Advert_Start_Date_For_MinimumWageType_For_Detail(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
            vacancyAdvert.WageText = "£15,704 to £25,396.80 a year";
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, null, true);
            
            actual.Should().Be("£15,704 to £25,396.80, depending on your age");
        }
        
        [Test, AutoData]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_No_Date_Of_Birth_And_Advert_Start_Date_For_MinimumWageType_For_Search(VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)WageType.NationalMinimumWage;
            vacancyAdvert.WageText = "£15,704 to £25,396.80 a year";
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, null, false);
            
            actual.Should().Be("£15,704 to £25,396.80 a year");
        }

        [Test]
        [InlineAutoData(WageType.FixedWage)]
        [InlineAutoData(WageType.CompetitiveSalary)]
        [InlineAutoData(WageType.Unknown)]
        public void GetVacancyAdvertWageText_Returns_Correct_Wage_For_Candidates_Date_Of_Birth_And_Advert_Start_Date_For_Fixed_And_Custom_WageType(
            WageType wageType,
            DateTime candidateDateOfBirth,
            VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)wageType;
                
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, candidateDateOfBirth);
            
            actual.Should().Be(vacancyAdvert.WageText);
            
        }
        
        [Test]
        [InlineAutoData(16, WageType.NationalMinimumWageForApprentices)]
        [InlineAutoData(16, WageType.NationalMinimumWage)]
        [InlineAutoData(19, WageType.NationalMinimumWage)]
        [InlineAutoData(23, WageType.NationalMinimumWage)]
        [InlineAutoData(25, WageType.NationalMinimumWage)]
        public void GetVacancyAdvertWageText_Returns_Correct_WageText_If_Null_For_Age_Based_Salaries_With_Candidates_Date_Of_Birth(
            int age,
            WageType wageType,
            VacancyAdvert vacancyAdvert)
        {
            vacancyAdvert.WageType = (int)wageType;
            vacancyAdvert.StartDate = DateTime.Now.AddYears(1);
            vacancyAdvert.ApprenticeMinimumWage = null;
            vacancyAdvert.Over25NationalMinimumWage = null;
            vacancyAdvert.Under18NationalMinimumWage = null;
            vacancyAdvert.Between18AndUnder21NationalMinimumWage = null;
            vacancyAdvert.Between21AndUnder25NationalMinimumWage = null;
            
            var actual = VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, DateTime.Now.AddYears(age * -1));
            
            actual.Should().Be(vacancyAdvert.WageText);
        }

        [Test]
        [InlineAutoData(WageType.NationalMinimumWageForApprentices,16,"National Minimum Wage rate for apprentices")]
        [InlineAutoData(WageType.FixedWage,16,"")]
        [InlineAutoData(WageType.CompetitiveSalary,16,"Competitive wage offered")]
        [InlineAutoData(WageType.NationalMinimumWage,16,"National Minimum Wage for an under 18 year old")]
        [InlineAutoData(WageType.NationalMinimumWage,23,"National Minimum Wage for a 24 year old")]
        [InlineAutoData(WageType.NationalMinimumWage,null,"National Minimum Wage")]
        public void GetVacancyAdvertDetailWageDescriptionText_Then_Gets_Wage_Text_For_Age_And_Wage_Type(
            WageType wageType,
            int? age,
            string expectedWageText)
        {
            var actual = VacancyDetailsHelperService.GetVacancyAdvertDetailWageDescriptionText(wageType,DateTime.Now.AddYears(1), age != null ? DateTime.Now.AddYears(age.Value * -1) : null);
            
            actual.Should().Be(expectedWageText);
        }
        
        [Test]
        [InlineAutoData(WageType.NationalMinimumWageForApprentices,16,"National Minimum Wage rate for apprentices")]
        [InlineAutoData(WageType.FixedWage,16,"")]
        [InlineAutoData(WageType.CompetitiveSalary,16,"Competitive wage offered")]
        [InlineAutoData(WageType.NationalMinimumWage,16,"National Minimum Wage for an under 18 year old")]
        [InlineAutoData(WageType.NationalMinimumWage,23,"National Minimum Wage for a 24 year old")]
        [InlineAutoData(WageType.NationalMinimumWage,null,"National Minimum Wage")]
        public void GetVacancyAdvertDetailWageText_Then_Gets_Wage_Text_For_Age_And_Wage_Type(
            WageType wageType,
            int? age,
            string expectedWageText)
        {
            var actual = VacancyDetailsHelperService.GetVacancyAdvertDetailWageDescriptionText(wageType,DateTime.Now.AddYears(1), age != null ? DateTime.Now.AddYears(age.Value * -1) : null);
            
            actual.Should().Be(expectedWageText);
        }

        [Test]
        [InlineAutoData("2000-01-30", "2020-01-29", 19)]
        [InlineAutoData("2000-01-30", "2020-01-31", 20)]
        [InlineAutoData("2000-01-30", "2020-12-31", 20)]
        [InlineAutoData("2000-01-30", "2021-01-01", 20)]
        public void GetCandidatesAgeAtStartDateOfVacancy_Then_Calculates_Candidate_Age_Based_On_Advert_Start_Date(string dateOfBirth, string startDate, int expectedAge)
        {
            var actualAge = VacancyDetailsHelperService.GetCandidatesAgeAtStartDateOfVacancy(DateTime.Parse(dateOfBirth), DateTime.Parse(startDate));

            actualAge.Should().Be(expectedAge);
        }

        
        [Test]
        [InlineAutoData("2024-06-01T15:30:00", true, "was closed at 3:30pm (GMT) on Saturday 1 June 2024")]
        [InlineAutoData("2024-12-25T00:00:00", false, "closed at 11:59pm on Wednesday 25 December 2024")]
        public void GetClosedDate_ReturnsFormattedString_WhenClosedDateHasValue(string dateString, bool isClosedEarly, string expected)
        {
            // Arrange
            DateTime closedDate = DateTime.Parse(dateString);

            // Act
            var result = VacancyDetailsHelperService.GetClosedDate(closedDate, isClosedEarly);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void GetClosedDate_ReturnsEmptyString_WhenClosedDateIsNull()
        {
            // Act
            var result = VacancyDetailsHelperService.GetClosedDate(null, false);

            // Assert
            result.Should().Be(string.Empty);
        }
    }
}