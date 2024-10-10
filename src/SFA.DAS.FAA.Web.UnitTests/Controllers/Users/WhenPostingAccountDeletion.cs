using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenPostingAccountDeletion
    {
        [Test, MoqAutoData]
        public void Then_Redirect_Is_Returned(
            string email,
            AccountDeletionViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            model.Email = email;
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
            var result = controller.AccountDeletion(model) as RedirectToRouteResult;
            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
        }

        [Test, MoqAutoData]
        public void And_Model_State_Is_Invalid_Should_Return_View_With_Model(
            AccountDeletionViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            controller.ModelState.AddModelError("SomeProperty", "SomeError");

            var result = controller.AccountDeletion(model) as ViewResult;

            result.Should().NotBeNull();
            result!.Model.Should().Be(model);
        }

        [Test, MoqAutoData]
        public void And_Email_Is_Different_Model_State_Is_Invalid_Should_Return_View_With_Model(
            string email,
            AccountDeletionViewModel model,
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

            controller.ModelState.Clear();

            var result = controller.AccountDeletion(model) as ViewResult;

            result.Should().NotBeNull();
            result.Model.Should().Be(model);
            controller.ModelState.ContainsKey(nameof(AccountDeletionViewModel.Email)).Should().BeTrue();
            controller.ModelState[nameof(AccountDeletionViewModel.Email)].Errors.Should().Contain(e => e.ErrorMessage == "This is not the email address you use with Find an apprenticeship. Check your email address and try again");
        }
    }
}
