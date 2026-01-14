using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Browse_By_Interests_View_Returned(
    GetBrowseByInterestsResult result,
    [Frozen] Mock<IMediator> mediator,
    [Greedy] SearchApprenticeshipsController controller)
    {
        // arrange
        mediator.Setup(x => x.Send(It.IsAny<GetBrowseByInterestsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // act
        var actual = await controller.BrowseByInterests() as ViewResult;

        // assert
        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as BrowseByInterestViewModel;
        actualModel!.Routes.Should().BeEquivalentTo(BrowseByInterestViewModel.ToViewModel(result).Routes);
    }
}
