using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UserName;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenPuttingUserName
    {
        //Complete when what is your date of birth page is created 
        
        //[Test, MoqAutoData]
        //public async Task When_Model_State_Is_Valid_Should_Redirect_To_What_Is_Your_Date_Of_Birth(
        // NameViewModel model,
        // [Frozen] Mock<IMediator> mediator,
        // [Greedy] UserController controller)
        //{
        //    // Arrange
        //    var result = await controller.Name(model) as RedirectToRouteResult;

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.RouteName.Should().Be(RouteNames.);
        //}

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
            NameViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            // Arrange
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

