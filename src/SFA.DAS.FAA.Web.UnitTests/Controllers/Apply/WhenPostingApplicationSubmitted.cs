using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply;

public class WhenPostingApplicationSubmitted
{
    [Test, MoqAutoData]
    public async Task And_User_Selected_Yes_Then_Redirect_To_Equality_Flow(
        ApplicationSubmittedViewModel model,
        string bannerMessage,
        string govIdentifier,
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResponse response,
        Mock<IValidator<ApplicationSubmittedViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] ApplyController controller)
    {
        // arrange
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(c =>
                c.CandidateId.Equals(candidateId)
                && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(ClaimTypes.NameIdentifier, govIdentifier)
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        model.AnswerEqualityQuestions = true;
        cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-ApplicationSubmitted"))
            .ReturnsAsync(bannerMessage);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<ApplicationSubmittedViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var result = await controller.ApplicationSubmitted(validator.Object, model) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        result!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender);
        cacheStorageService.Verify(x => x.Set($"{govIdentifier}-ApplicationSubmitted", It.IsAny<object>(), 1, 1), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_User_Selected_No_Then_Redirect_To_Their_Applications(
        ApplicationSubmittedViewModel model,
        string bannerMessage,
        string govIdentifier,
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResponse response,
        Mock<IValidator<ApplicationSubmittedViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] ApplyController controller)
    {
        // arrange
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(c =>
                c.CandidateId.Equals(candidateId)
                && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(ClaimTypes.NameIdentifier, govIdentifier)
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        model.AnswerEqualityQuestions = false;
        cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-ApplicationSubmitted"))
            .ReturnsAsync(bannerMessage);

        validator
            .Setup(x => x.ValidateAsync(It.Is<ApplicationSubmittedViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        // act
        var result = await controller.ApplicationSubmitted(validator.Object, model) as RedirectToRouteResult;

        // assert
        result.Should().NotBeNull();
        result!.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        cacheStorageService.Verify(x => x.Set($"{govIdentifier}-ApplicationSubmitted", It.IsAny<object>(), 1, 1), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Valid_View_Returned(
        ApplicationSubmittedViewModel model,
        Guid candidateId,
        Guid applicationId,
        GetApplicationSubmittedQueryResponse response,
        Mock<IValidator<ApplicationSubmittedViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplyController controller)
    {
        // arrange
        mediator.Setup(x => x.Send(It.Is<GetApplicationSubmittedQuery>(c =>
                c.CandidateId.Equals(candidateId)
                && c.ApplicationId.Equals(applicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var user = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(CustomClaims.CandidateId, candidateId.ToString()),
        ]));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        controller.ModelState.AddModelError("Error", "Error");
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<ApplicationSubmittedViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.ApplicationSubmitted(validator.Object, model) as ViewResult;
        
        // assert
        actual.Should().NotBeNull();
        var actualModel = actual!.Model as ApplicationSubmittedViewModel;
        actualModel!.ApplicationId.Should().Be(model.ApplicationId);
    }
}