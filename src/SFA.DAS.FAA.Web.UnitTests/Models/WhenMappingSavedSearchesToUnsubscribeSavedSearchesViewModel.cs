using SFA.DAS.FAA.Application.Queries.SavedSearches;
using SFA.DAS.FAA.Web.Models.SavedSearches;

namespace SFA.DAS.FAA.Web.UnitTests.Models
{
    public class WhenMappingSavedSearchesToUnsubscribeSavedSearchesViewModel
    {
        [Test, MoqAutoData]
        public void Map_Returns_Expected_Results(GetConfirmUnsubscribeResult source)
        {
            var result = (UnsubscribeSavedSearchesViewModel?)source;

            result?.Id.Should().Equals(source.SavedSearch?.Id);
            result?.SearchTitle.Should().BeEquivalentTo(source.SavedSearch?.SearchTitle);
            result?.What.Should().BeEquivalentTo(source.SavedSearch?.What);
            result?.Where.Should().BeEquivalentTo(source.SavedSearch?.Where);
            result?.Distance.Should().Equals(source.SavedSearch?.Distance);
            result?.Categories.Should().BeEquivalentTo(source.SavedSearch?.Categories);
            result?.Levels.Should().BeEquivalentTo(source.SavedSearch?.Levels);
            result?.DisabilityConfident.Should().Equals(source.SavedSearch?.DisabilityConfident);
        }
    }
}
