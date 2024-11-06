using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.SavedSearches.GetSavedSearches;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenGettingSavedSearches
{
    [Test, MoqAutoData]
    public async Task Then_The_View_Is_Returned(
        Guid candidateId,
        [Frozen] GetSavedSearchesQueryResult savedSearchesQueryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController sut
        )
    {
        // arrange
        sut.AddControllerContext().WithUser(candidateId);
        
        mediator
            .Setup(x => x.Send(It.IsAny<GetSavedSearchesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(savedSearchesQueryResult);
        
        // act
        var actual = (await sut.GetSavedSearches(default) as ViewResult)?.Model as SavedSearchesViewModel;
        
        // assert
        actual.Should().NotBeNull();
        actual!.SavedSearches.Count.Should().Be(3);
        actual.SavedSearches[0].Should().BeEquivalentTo(savedSearchesQueryResult.SavedSearches[0], options => options.ExcludingMissingMembers());
    }
}