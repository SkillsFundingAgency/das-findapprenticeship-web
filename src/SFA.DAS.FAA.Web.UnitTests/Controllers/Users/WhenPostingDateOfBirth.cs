using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UserDateOfBirth;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;
public class WhenPostingDateOfBirth
{
    [Test, MoqAutoData]
    public async Task When_Model_State_Is_Valid_Should_Redirect_To_Search_Results(
         Guid candidateId,
         string govIdentifier,
         string email,
         DateOfBirthViewModel model,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
    {
        model.ReturnToConfirmationPage = false; 
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))
            }
        };

        var result = await controller.DateOfBirth(model) as RedirectToRouteResult;

        result.Should().NotBeNull();
        result.RouteName.Should().Be(RouteNames.PostcodeAddress);
        mediator.Verify(x => x.Send(It.Is<UpdateDateOfBirthCommand>(c =>
            c.CandidateId.Equals(candidateId)
            && c.DateOfBirth.Equals(model.DateOfBirth.DateTimeValue.Value)
            && c.Email.Equals(email)
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Name_When_Model_State_Is_Invalid_Should_Return_View_With_Model(
         DateOfBirthViewModel model,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
    {
        controller.ModelState.AddModelError("SomeProperty", "SomeError");

        var result = await controller.DateOfBirth(model) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Test, MoqAutoData]
    public async Task Name_When_Mediator_Send_Throws_InvalidOperationException_Should_Return_View_With_Model_Error(
         string govIdentifier,
         string email,
         DateOfBirthViewModel model,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        new Claim(ClaimTypes.Email, email),
                    }))

            }
        };
        mediator.Setup(x => x.Send(It.IsAny<UpdateDateOfBirthCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        var result = await controller.DateOfBirth(model) as ViewResult;

        result.Should().NotBeNull();
        result.Model.Should().Be(model);
        controller.ModelState.Count.Should().BeGreaterThan(0);
    }
}
