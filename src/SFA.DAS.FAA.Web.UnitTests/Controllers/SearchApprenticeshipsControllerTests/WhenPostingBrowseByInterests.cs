using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenPostingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task AndModelStateIsInvalid_ModelIsReturned(
        GetBrowseByInterestsResult response,
        BrowseByInterestViewModel model,
        Mock<IValidator<BrowseByInterestViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        // arrange
        validator
            .Setup(x => x.ValidateAsync(It.Is<BrowseByInterestViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("test", "message")]));
        
        mediator.Setup(x => x.Send(It.IsAny<GetBrowseByInterestsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        var result = await controller.BrowseByInterests(validator.Object, model) as ViewResult;

        // assert
        result!.Model.Should().BeOfType<BrowseByInterestViewModel>();
    }

    [Test, MoqAutoData]
    public async Task And_ThereIsNoValidationError_LocationsPageIsReturned(
        Mock<IValidator<BrowseByInterestViewModel>> validator,
        BrowseByInterestViewModel model,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        // arrange
        validator
            .Setup(x => x.ValidateAsync(It.Is<BrowseByInterestViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        // act
        var result = await controller.BrowseByInterests(validator.Object, model) as ActionResult;

        // assert
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Location);
    }
}