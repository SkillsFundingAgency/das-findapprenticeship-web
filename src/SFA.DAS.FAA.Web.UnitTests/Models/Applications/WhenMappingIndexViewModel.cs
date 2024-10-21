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
        IDateTimeService _dateTimeService;

        [SetUp]
        public void Setup()
        {
            var dts = new Mock<IDateTimeService>();
            dts.Setup(x => x.GetDateTime()).Returns(DateTime.UtcNow);

            _dateTimeService = dts.Object;
        }

        [Test, MoqAutoData]
        public void Then_A_Vacancy_Closed_Before_Today_Generates_The_Correct_Closed_Text(IFixture fixture)
        {
            // arrange
            var application = fixture.Create<GetIndexQueryResult.Application>();
            application.ClosingDate = DateTime.UtcNow.AddDays(5);
            application.ClosedDate = DateTime.UtcNow.AddDays(-5);
            application.Status = ApplicationStatus.Submitted;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService);

            // assert
            actual.IsClosed.Should().BeTrue();
            actual.ClosingDate.Should().StartWith("Closed");
        }

        [Test, MoqAutoData]
        public void Then_A_Vacancy_Closed_Early_Today_Generates_The_Correct_Closed_Text(IFixture fixture)
        {
            // arrange
            var application = fixture.Create<GetIndexQueryResult.Application>();
            application.ClosingDate = DateTime.UtcNow.AddDays(5);
            application.ClosedDate = DateTime.UtcNow.AddSeconds(-5);
            application.Status = ApplicationStatus.Submitted;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService);

            // assert
            actual.IsClosed.Should().BeTrue();
            actual.ClosingDate.Should().StartWith("Closed");
        }

        [Test, MoqAutoData]
        public void Then_A_Vacancy_Closing_Today_Generates_The_Correct_Closed_Text(IFixture fixture)
        {
            // arrange
            var application = fixture.Create<GetIndexQueryResult.Application>();
            application.ClosingDate = DateTime.Today.AsUtc();
            application.ClosedDate = null;
            application.Status = ApplicationStatus.Submitted;

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService);

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
        public void Then_A_Closed_Vacancy_Is_Not_Closing_Soon(int days, IFixture fixture)
        {
            // arrange
            var application = fixture.Create<GetIndexQueryResult.Application>();
            application.ClosedDate = DateTime.Today.AddDays(-10).AsUtc();
            application.ClosingDate = DateTime.Today.AddDays(days).AsUtc();

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService);

            // assert
            actual.IsClosingSoon.Should().BeFalse();
        }
        
        [Test]
        [InlineAutoData(0, true)]
        [InlineAutoData(3, true)]
        [InlineAutoData(7, true)]
        [InlineAutoData(8, false)]
        public void Then_An_Open_Vacancy_Is_Closing_Soon_If_Closing_Date_Within_7_Days(int days, bool expected, IFixture fixture)
        {
            // arrange
            var application = fixture.Create<GetIndexQueryResult.Application>();
            application.ClosedDate = null;
            application.ClosingDate = DateTime.Today.AddDays(days).AsUtc();

            // act
            var actual = IndexViewModel.Application.From(application, _dateTimeService);

            // assert
            actual.IsClosingSoon.Should().Be(expected);
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