using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserName;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

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
        Mock<IValidator<NameViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // Arrange
        model.JourneyPath = journeyPath;
        controller.WithContext(x => x
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier)
        );
            
        validator
            .Setup(x => x.ValidateAsync(It.Is<NameViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
            
        // Act
        var result = await controller.Name(validator.Object, model) as RedirectToRouteResult;

        // Assert
        result.Should().NotBeNull();
        result!.RouteName.Should().Be(redirectRoute);
        result.RouteValues["journeyPath"].Should().Be(journeyPath);
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
        Mock<IValidator<NameViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // Arrange
        validator
            .Setup(x => x.ValidateAsync(It.Is<NameViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("SomeProperty", "SomeError")]));

        // Act
        var result = await controller.Name(validator.Object, model) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Test, MoqAutoData]
    public async Task Name_When_Mediator_Send_Throws_InvalidOperationException_Should_Return_View_With_Model_Error(
        string govIdentifier,
        string email,
        NameViewModel model,
        Mock<IValidator<NameViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] UserController controller)
    {
        // Arrange
        controller.WithContext(x => x
            .WithUser(Guid.NewGuid())
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier)
        );
        mediator.Setup(x => x.Send(It.IsAny<UpdateNameCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        validator
            .Setup(x => x.ValidateAsync(It.Is<NameViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // Act
        var result = await controller.Name(validator.Object, model) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().Be(model);
        controller.ModelState.Count.Should().BeGreaterThan(0);
    }
}