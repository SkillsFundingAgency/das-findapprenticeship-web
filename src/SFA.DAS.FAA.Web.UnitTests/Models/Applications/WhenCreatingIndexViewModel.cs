using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Applications
{
    [TestFixture]
    public class WhenCreatingIndexViewModel
    {
        [Test]
        [MoqInlineAutoData("2024-04-18", "2024-03-17T23:59:59", "Closes on Thursday 18 April 2024")]
        [MoqInlineAutoData("2024-04-18", "2024-03-18T00:00:01", "Closes in 31 days (Thursday 18 April 2024 at 11:59pm)")]
        [MoqInlineAutoData("2024-04-18", "2024-04-03T08:30", "Closes in 15 days (Thursday 18 April 2024 at 11:59pm)")]
        [MoqInlineAutoData("2024-04-18", "2024-04-17T19:41", "Closes tomorrow (Thursday 18 April 2024 at 11:59pm)")]
        [MoqInlineAutoData("2024-04-18", "2024-04-18T19:41", "Closes today at 11:59pm")]
        [MoqInlineAutoData("2024-04-18", "2024-04-19T00:01:01", "Closed on Thursday 18 April 2024")]
        [MoqInlineAutoData("2024-04-03", "2024-04-19T00:01:01", "Closed on Wednesday 3 April 2024")]
        public void Then_Application_ClosingDate_Is_Expected_Value(
            string closingDate,
            string currentDate,
            string expectedValue,
            ApprenticeshipTypes apprenticeshipType)
        {
            var queryResult = new GetIndexQueryResult
            {
                Applications =
                [
                    new()
                    {
                        CreatedDate = DateTime.UtcNow,
                        ClosingDate = Convert.ToDateTime(closingDate),
                        ApprenticeshipType = apprenticeshipType

                    }
                ]
            };

            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(x => x.GetDateTime()).Returns(Convert.ToDateTime(currentDate));

            var result = IndexViewModel.Map(ApplicationsTab.Started, queryResult, dateTimeService.Object);

            var application = result.Applications.SingleOrDefault() ?? result.ExpiredApplications.First();
            application.ClosingDate.Should().Be(expectedValue);
            application.ApprenticeshipType.Should().Be(apprenticeshipType);
        }
    }
}
