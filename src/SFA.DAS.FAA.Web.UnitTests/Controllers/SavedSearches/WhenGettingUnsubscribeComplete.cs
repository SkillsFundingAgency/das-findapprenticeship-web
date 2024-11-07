using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SavedSearches;

public class WhenGettingUnsubscribeComplete
{
    [Test, MoqAutoData]
    public async Task Then_The_Confirmation_Is_Shown(
        Guid savedSearchId,
        string searchTitle,
        UnsubscribeSavedSearchesModel postModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        cacheStorageService.Setup(x => 
            x.Get<string>($"{savedSearchId}-savedsearch")).ReturnsAsync(searchTitle);
        
        var actual = await savedSearchesController.UnsubscribeComplete(savedSearchId) as ViewResult;

        actual.Should().NotBeNull();
        
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Value_In_Cache_Redirect_To_Home(
        Guid savedSearchId,
        string searchTitle,
        UnsubscribeSavedSearchesModel postModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        cacheStorageService.Setup(x => 
            x.Get<string?>($"{savedSearchId}-savedsearch")).ReturnsAsync((string)null!);
        
        var actual = await savedSearchesController.UnsubscribeComplete(savedSearchId) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
}