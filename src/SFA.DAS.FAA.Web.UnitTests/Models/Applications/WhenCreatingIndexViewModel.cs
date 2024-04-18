using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Applications
{
    [TestFixture]
    public class WhenCreatingIndexViewModel
    {
        [TestCase("2024-04-18", "2024-03-17T23:59:59", "Closes on Thursday 18 April 2024 at 11:59pm")]
        [TestCase("2024-04-18", "2024-03-18T00:00:01", "Closes in 31 days (Thursday 18 April 2024 at 11:59pm)")]
        [TestCase("2024-04-18", "2024-04-03T08:30", "Closes in 15 days (Thursday 18 April 2024 at 11:59pm)")]
        [TestCase("2024-04-18", "2024-04-17T19:41", "Closes tomorrow (Thursday 18 April 2024 at 11:59pm)")]
        [TestCase("2024-04-18", "2024-04-18T19:41", "Closes today at 11:59pm")]
        public void Then_Application_ClosingDate_Is_Expected_Value(DateTime closingDate, DateTime currentDate, string expectedValue)
        {
            closingDate = closingDate.AddDays(1).Date.Subtract(new TimeSpan(0,0,1));

            var queryResult = new GetIndexQueryResult
            {
                Applications =
                [
                    new()
                    {
                        CreatedDate = DateTime.UtcNow,
                        ClosingDate = closingDate
                    }
                ]
            };

            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(currentDate);

            var result = IndexViewModel.Create(ApplicationsTab.Started, queryResult, dateTimeService.Object);

            result.Applications.First().ClosingDate.Should().Be(expectedValue);
        }
    }
}
