using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.FAA.Application.Commands.User.PostAccountDeletion;
using SFA.DAS.FAA.Web.AppStart;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

[TestFixture]
public class WhenPostingAccountDeletion
{
    [Test, MoqAutoData]
    public async Task Then_Redirect_Is_Returned(
        Guid candidateId,
        string email,
        AccountDeletionViewModel model,
        Mock<IValidator<AccountDeletionViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(CustomClaims.CandidateId, candidateId.ToString()),
            })),
        };
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            [CacheKeys.AccountDeleted] = "true"
        };
        model.Email = email;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext,
        };
        controller.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            [CacheKeys.AccountDeleted] = "true"
        };

        validator
            .Setup(x => x.ValidateAsync(It.IsAny<AccountDeletionViewModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var result = await controller.AccountDeletion(validator.Object, model) as RedirectToRouteResult;
        result.Should().NotBeNull();
        result!.RouteName.Should().Be(RouteNames.SignOut);

        mediator.Verify(x => x.Send(It.IsAny<AccountDeletionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Model_State_Is_Invalid_Should_Return_View_With_Model(
        AccountDeletionViewModel model,
        Mock<IValidator<AccountDeletionViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        validator
            .Setup(x => x.ValidateAsync(It.IsAny<AccountDeletionViewModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        controller.ModelState.AddModelError("SomeProperty", "SomeError");

        var result = await controller.AccountDeletion(validator.Object, model) as ViewResult;

        result.Should().NotBeNull();
        result!.Model.Should().Be(model);
    }

    [Test, MoqAutoData]
    public async Task And_Email_Is_Different_Model_State_Is_Invalid_Should_Return_View_With_Model(
        string email,
        AccountDeletionViewModel model,
        Mock<IValidator<AccountDeletionViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                }))

            }
        };

        validator
            .Setup(x => x.ValidateAsync(It.IsAny<AccountDeletionViewModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        controller.ModelState.Clear();

        var result = await controller.AccountDeletion(validator.Object, model) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
        controller.ModelState.ContainsKey(nameof(AccountDeletionViewModel.Email)).Should().BeTrue();
        controller.ModelState[nameof(AccountDeletionViewModel.Email)].Errors.Should().Contain(e => e.ErrorMessage == "This is not the email address you use with Find an apprenticeship. Check your email address and try again");
    }
}