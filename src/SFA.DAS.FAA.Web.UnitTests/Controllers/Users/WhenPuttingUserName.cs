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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserName;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenPuttingUserName
    {
        [Test]
        [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.DateOfBirth)]
        [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails)]
        [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings)]
        public async Task When_Model_State_Is_Valid_Should_Redirect_To_What_Is_Your_Date_Of_Birth(
         UserJourneyPath journeyPath,
         string redirectRoute,
         string govIdentifier,
         Guid candidateId,
         string email,
         NameViewModel model,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
        {
            //Arrange
            model.JourneyPath = journeyPath;
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
            
            // Act
            var result = await controller.Name(model) as RedirectToRouteResult;

            // Assert
            result.Should().NotBeNull();
            result!.RouteName.Should().Be(redirectRoute);
            mediator.Verify(x => x.Send(It.Is<UpdateNameCommand>(c =>
                c.CandidateId.Equals(candidateId)
                && c.FirstName.Equals(model.FirstName)
                && c.LastName.Equals(model.LastName)
                && c.Email.Equals(email)
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Name_When_Model_State_Is_Invalid_Should_Return_View_With_Model(
            NameViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            // Arrange
            controller.ModelState.AddModelError("SomeProperty", "SomeError");

            // Act
            var result = await controller.Name(model) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().Be(model);
        }

        [Test, MoqAutoData]
        public async Task Name_When_Mediator_Send_Throws_InvalidOperationException_Should_Return_View_With_Model_Error(
            string govIdentifier,
            string email,
            NameViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            // Arrange
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
            mediator.Setup(x => x.Send(It.IsAny<UpdateNameCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await controller.Name(model) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().Be(model);
            controller.ModelState.Count.Should().BeGreaterThan(0);
        }
    }
}

