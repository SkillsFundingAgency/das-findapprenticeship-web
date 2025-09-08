using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.CreateAccount.UserDateOfBirth;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenPostingDateOfBirth
{
    [Test]
    [MoqInlineAutoData(UserJourneyPath.CreateAccount, RouteNames.PostcodeAddress)]
    [MoqInlineAutoData(UserJourneyPath.ConfirmAccountDetails, RouteNames.ConfirmAccountDetails)]
    [MoqInlineAutoData(UserJourneyPath.Settings, RouteNames.Settings)]
    public async Task When_Model_State_Is_Valid_Should_Redirect(
         UserJourneyPath journeyPath,
         string redirectRoute,
         Guid candidateId,
         string govIdentifier,
         string email,
         DateOfBirthViewModel model,
         Mock<IValidator<DateOfBirthViewModel>> validator,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
    {
        // arrange
        model.JourneyPath = journeyPath; 
        controller
            .AddControllerContext()
            .WithUser(candidateId)
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<DateOfBirthViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.DateOfBirth(validator.Object, model) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        result.RouteName.Should().Be(redirectRoute);
        result.RouteValues["journeyPath"].Should().Be(journeyPath);
        mediator.Verify(x => x.Send(It.Is<UpdateDateOfBirthCommand>(c =>
            c.CandidateId.Equals(candidateId)
            && c.DateOfBirth.Equals(model.DateOfBirth.DateTimeValue.Value)
            && c.Email.Equals(email)
            ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Name_When_Model_State_Is_Invalid_Should_Return_View_With_Model(
         DateOfBirthViewModel model,
         Mock<IValidator<DateOfBirthViewModel>> validator,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
    {
        // arrange
        validator
            .Setup(x => x.ValidateAsync(It.Is<DateOfBirthViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("SomeProperty", "SomeError")]));
        
        // act
        var result = await controller.DateOfBirth(validator.Object, model) as ViewResult;

        // assert
        result.Should().NotBeNull();
        result.Model.Should().Be(model);
    }

    [Test, MoqAutoData]
    public async Task Name_When_Mediator_Send_Throws_InvalidOperationException_Should_Return_View_With_Model_Error(
         string govIdentifier,
         string email,
         DateOfBirthViewModel model,
         Mock<IValidator<DateOfBirthViewModel>> validator,
         [Frozen] Mock<IMediator> mediator,
         [Greedy] UserController controller)
    {
        // arrange
        controller
            .AddControllerContext()
            .WithUser(Guid.NewGuid())
            .WithClaim(ClaimTypes.Email, email)
            .WithClaim(ClaimTypes.NameIdentifier, govIdentifier);
        
        mediator.Setup(x => x.Send(It.IsAny<UpdateDateOfBirthCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<DateOfBirthViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.DateOfBirth(validator.Object, model) as ViewResult;

        // assert
        result.Should().NotBeNull();
        result.Model.Should().Be(model);
        controller.ModelState.Count.Should().BeGreaterThan(0);
    }
}
