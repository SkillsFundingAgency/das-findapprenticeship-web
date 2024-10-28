using AutoFixture;
using AutoFixture.NUnit3;
using SFA.DAS.FAA.Application.Queries.Applications.GetIndex;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Applications
{
    public class WhenMappingIndexViewModel
    {
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
}
