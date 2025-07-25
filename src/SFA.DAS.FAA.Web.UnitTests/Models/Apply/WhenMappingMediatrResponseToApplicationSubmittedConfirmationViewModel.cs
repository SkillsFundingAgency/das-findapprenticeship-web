using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply
{
    public class WhenMappingMediatrResponseToApplicationSubmittedConfirmationViewModel
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetApplicationSubmittedQueryResponse source)
        {
            var actual = (ApplicationSubmittedVacancyInfo)source;

            actual.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }
    }
}
