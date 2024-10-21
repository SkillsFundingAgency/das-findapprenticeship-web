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