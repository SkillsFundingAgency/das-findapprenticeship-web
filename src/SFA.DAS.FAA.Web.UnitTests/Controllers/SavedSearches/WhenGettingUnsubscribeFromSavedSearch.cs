using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SavedSearches;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SavedSearches;

public class WhenGettingUnsubscribeFromSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_The_SearchAlert_Is_Retrieved_And_SearchDetails_Returned(
        Guid savedSearchId,
        string id,
        GetConfirmUnsubscribeResult queryResult,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        mediator.Setup(x => x.Send(It.Is<GetConfirmUnsubscribeQuery>(c => c.SavedSearchId.Equals(savedSearchId)),
            It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);
        
        dataProtectorService.Setup(x=>x.DecodeData(id)).Returns(savedSearchId.ToString());
        
        var actual = await savedSearchesController.Unsubscribe(id) as ViewResult;

        actual.Should().NotBeNull();
        var actualModel = actual!.Model as UnsubscribeSavedSearchesViewModel;
        actualModel.Should().NotBeNull();
        actualModel!.SavedSearch.Should().BeEquivalentTo(SavedSearchViewModel.From(queryResult.SavedSearch!, queryResult.Routes));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Id_Can_Not_Be_Decoded_Then_Redirect_To_Home_Page(
        Guid savedSearchId,
        string id,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        dataProtectorService.Setup(x=>x.DecodeData(id)).Returns((string?)null);
        
        var actual = await savedSearchesController.Unsubscribe(id) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Id_Decoded_Is_Not_A_Guid_Then_Redirect_To_Home_Page(
        string someOtherId,
        string id,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        dataProtectorService.Setup(x=>x.DecodeData(id)).Returns(someOtherId);
        
        var actual = await savedSearchesController.Unsubscribe(id) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_SearchAlert_Is_Not_Found_Then_Redirect_To_HomePage(
        Guid savedSearchId,
        string id,
        [Frozen] Mock<IDataProtectorService> dataProtectorService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SavedSearchesController savedSearchesController)
    {
        mediator.Setup(x => x.Send(It.Is<GetConfirmUnsubscribeQuery>(c => c.SavedSearchId.Equals(savedSearchId)),
            It.IsAny<CancellationToken>())).ReturnsAsync(new GetConfirmUnsubscribeResult());
        dataProtectorService.Setup(x=>x.DecodeData(id)).Returns(savedSearchId.ToString());
        
        var actual = await savedSearchesController.Unsubscribe(id) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
}