using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenPostingDeleteSavedSearch
{
    [Test, MoqAutoData]
    public async Task Then_A_Successful_Delete_Redirects_Back_To_The_Saved_Searches_Route(
        Guid candidateId,
        Guid savedSearchId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
    )
    {
        // arrange
        sut.AddControllerContext().WithUser(candidateId);
        
        // act
        var response = await sut.DeleteSavedSearch(savedSearchId, default) as RedirectToRouteResult;
        
        // assert
        response.Should().NotBeNull();
        response!.RouteName.Should().Be(RouteNames.SavedSearches);
    }
}