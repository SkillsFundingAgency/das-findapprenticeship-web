using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenPostingConfirmYourAccountDetails
{
    [Test, MoqAutoData]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Search_Results(
     ConfirmAccountDetailsViewModel model,
     [Frozen] Mock<IMediator> mediator,
     [Greedy] UserController controller)
    {
        var result = controller.ConfirmYourAccountDetails(model) as RedirectToRouteResult;

        result.Should().NotBeNull();
        result.RouteName.Should().Be(RouteNames.Apply);
    }
}
