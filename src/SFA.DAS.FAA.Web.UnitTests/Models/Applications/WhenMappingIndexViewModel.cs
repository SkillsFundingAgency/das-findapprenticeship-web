using FluentAssertions.Extensions;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Applications;

[TestFixture]
public class WhenMappingIndexViewModel
{
    public class WhenMappingApplication
    {
        private Mock<IDateTimeService> _dateTimeService;
        private static readonly DateTime Today = new DateTime(2024, 1, 1).AsUtc();

        [SetUp]
        public void Setup()
        {
            _dateTimeService = new Mock<IDateTimeService>();
            _dateTimeService.Setup(x => x.GetDateTime()).Returns(Today.AsUtc());
        }

        [Test, MoqAutoData]
        public void Then_A_Vacancy_Closed_Before_Today_Generates_The_Correct_Closed_Text(GetIndexQueryResult.Application application)
        {
            // arrange
            application.ClosingDate = Today.AddDays(5);
            application.ClosedDate = Today.AddDays(-5);
            application.Status = ApplicationStatus.Submitted;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService.Object);

            // assert
            actual.IsClosed.Should().BeTrue();
            actual.ClosingDate.Should().StartWith("Closed");
        }

        [Test, MoqAutoData]
        public void Then_A_Vacancy_Closed_Early_Today_Generates_The_Correct_Closed_Text(GetIndexQueryResult.Application application)
        {
            // arrange
            application.ClosingDate = Today.AddDays(5);
            application.ClosedDate = Today.AddSeconds(-5);
            application.Status = ApplicationStatus.Submitted;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService.Object);

            // assert
            actual.IsClosed.Should().BeTrue();
            actual.ClosingDate.Should().StartWith("Closed");
        }

        [Test, MoqAutoData]
        public void Then_A_Vacancy_Closing_Today_Generates_The_Correct_Closed_Text(GetIndexQueryResult.Application application)
        {
            // arrange
            application.ClosingDate = Today;
            application.ClosedDate = null;
            application.Status = ApplicationStatus.Submitted;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService.Object);

            // assert
            actual.IsClosed.Should().BeFalse();
            actual.ClosingDate.Should().StartWith("Closes today");
        }

        [Test]
        [InlineAutoData(-1)]
        [InlineAutoData(0)]
        [InlineAutoData(3)]
        [InlineAutoData(7)]
        [InlineAutoData(14)]
        [InlineAutoData(21)]
        public void Then_A_Closed_Vacancy_Is_Not_Closing_Soon(int days, GetIndexQueryResult.Application application)
        {
            // arrange
            application.ClosedDate = Today.AddDays(-10);
            application.ClosingDate = Today.AddDays(days);

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService.Object);

            // assert
            actual.IsClosingSoon.Should().BeFalse();
        }
        
        [Test]
        [InlineAutoData(0, true)]
        [InlineAutoData(3, true)]
        [InlineAutoData(7, true)]
        [InlineAutoData(8, false)]
        public void Then_An_Open_Vacancy_Is_Closing_Soon_If_Closing_Date_Within_7_Days(int days, bool expected, GetIndexQueryResult.Application application)
        {
            // arrange
            application.ClosedDate = null;
            application.ClosingDate = Today.AddDays(days);

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService.Object);

            // assert
            actual.IsClosingSoon.Should().Be(expected);
        }

        [Test]
        [InlineAutoData(ApplicationStatus.Draft, "")]
        [InlineAutoData(ApplicationStatus.Submitted, "")]
        [InlineAutoData(ApplicationStatus.Withdrawn, "")]
        [InlineAutoData(ApplicationStatus.Successful, "Offered on 1 January 2024")]
        [InlineAutoData(ApplicationStatus.Unsuccessful, "Feedback received on 1 January 2024")]
        [InlineAutoData(ApplicationStatus.Expired, "")]
        public void Then_The_ResponseDate_Text_Is_Set_From_The_Application_Status(ApplicationStatus applicationStatus, string expected, GetIndexQueryResult.Application application)
        {
            // arrange
            application.Status = applicationStatus;
            application.SubmittedDate = Today;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService.Object);

            // assert
            actual.ResponseDate.Should().Be(expected);
            actual.ApprenticeshipType.Should().Be(application.ApprenticeshipType);
            actual.ShowFoundationTag.Should().Be(application.ApprenticeshipType == ApprenticeshipTypes.Foundation);
        }
    }
    
    [Test, MoqAutoData]
    public void Then_Applications_Are_Correctly_Split_By_Their_Status_Into_Their_Associated_Properties(
        IFixture fixture,
        GetIndexQueryResult source,
        [Frozen] IDateTimeService dateTimeService)
    {
        // arrange - let's make sure the application statuses are consistent 
        source.Applications.Clear();
        var applicationStatusValues = Enum.GetValues<ApplicationStatus>().ToList();
        applicationStatusValues.ForEach(x =>
        {
            var application = fixture.Create<GetIndexQueryResult.Application>();
            application.Status = x;
            source.Applications.Add(application);
        });

        // act
        var result = IndexViewModel.Map(ApplicationsTab.None, source, dateTimeService);
            
        // assert
        using (new AssertionScope())
        {
            result.Applications.Count.Should().Be(4);
            result.WithdrawnApplications.Count.Should().Be(1);
            result.ExpiredApplications.Count.Should().Be(1);
        }
    }
}