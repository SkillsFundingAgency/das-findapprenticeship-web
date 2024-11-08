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
        UnsubscribeSavedSearchesModel postModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        var actual = await savedSearchesController.Index(postModel) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.UnsubscribeSavedSearchComplete);
        actual.RouteValues!["id"].Should().Be(postModel.Id);
        mediator.Verify(
            x=>x.Send(
                It.Is<UnsubscribeSavedSearchCommand>(
                    c => c.SavedSearchId.Equals(postModel.Id)
                    ), It.IsAny<CancellationToken>()
                ), Times.Once);
        cacheStorageService.Verify(x => 
            x.Set($"{postModel.Id}-savedsearch", postModel, 5, 5), Times.Once);
    }
}