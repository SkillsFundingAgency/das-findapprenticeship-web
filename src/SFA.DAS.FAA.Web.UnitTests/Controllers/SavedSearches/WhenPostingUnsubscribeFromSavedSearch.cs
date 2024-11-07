using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;
using SFA.DAS.FAA.Application.Commands.SavedSearches.DeleteSavedSearch;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SavedSearches;

public class WhenPostingUnsubscribeFromSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_SearchAlert_Command_Is_Made_And_Redirect_To_Confirmation(
        Guid savedSearchId,
        UnsubscribeSavedSearchesModel postModel,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        var actual = await savedSearchesController.Index(postModel) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.PostSavedSearchesUnsubscribe);
        mediator.Verify(
            x=>x.Send(
                It.Is<UnsubscribeSavedSearchCommand>(
                    c => c.SavedSearchId.Equals(postModel.Id)
                    ), It.IsAny<CancellationToken>()
                ), Times.Once);
    }
}