using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Filters;
using SFA.DAS.FAA.Web.UnitTests.Customisations;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using SFA.DAS.FAA.Web.Attributes;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Filters;

public class WhenFilteringNewAccountUsers
{
    [Test, MoqAutoData]
    public async Task And_Has_Candidate_And_Account_Setup_Completed_The_Continues(
        [ArrangeActionContext<SearchApprenticeshipsController>] ActionExecutingContext context,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        context.ActionDescriptor = new ControllerActionDescriptor
        {
            MethodInfo = typeof(DummyExemptionClass).GetMethod(nameof(DummyExemptionClass.NoExemption))!
        };
        context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(CustomClaims.AccountSetupCompleted, "true"),
            new Claim(CustomClaims.CandidateId, "Candidate1")
        }));
            
        //Act
        await filter.OnActionExecutionAsync(context, nextMethod.Object);

        //Assert
        nextMethod.Verify( x => x(), Times.Once);
        Assert.That(context.Result ,Is.Null);
    }
    
    [Test]
    [MoqInlineAutoData(UserStatus.Completed, true)]
    [MoqInlineAutoData(UserStatus.Incomplete, false)]
    public async Task And_Has_CandidateId_And_NameIdentifier_User_Status_Returns_ViewData(
        UserStatus status,
        bool expectedViewData,
        string email,
        string userId,
        PutCandidateApiResponse response,
        [ArrangeActionContext<SearchApprenticeshipsController>] ActionExecutingContext context,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        [Frozen] Mock<IApiClient> apiClient,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        response.Status = status;
        var httpContext = new Mock<HttpContext>();
        context.ActionDescriptor = new ControllerActionDescriptor
        {
            MethodInfo = typeof(DummyExemptionClass).GetMethod(nameof(DummyExemptionClass.NoExemption))!
        };

        httpContext.Setup(x => x.RequestServices.GetService(typeof(IApiClient)))
            .Returns(apiClient.Object);

        httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId)
        })));

        var request = new PutCandidateApiRequest(userId, new PutCandidateApiRequestData
        {
            Email = email
        });
        apiClient.Setup(x =>
                x.Put<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(request.PutUrl)
                                                       && ((PutCandidateApiRequestData)c.Data).Email == email
        ))).ReturnsAsync(response);

        context.HttpContext = httpContext.Object;

        //Act
        await filter.OnActionExecutionAsync(context, nextMethod.Object);
        ((Controller) context.Controller).ViewData["IsAccountStatusCompleted"].Should().NotBeNull();
        ((Controller) context.Controller).ViewData["IsAccountStatusCompleted"].Should().Be(expectedViewData);
    }

    [Test, MoqAutoData]
    public async Task And_Has_No_CandidateId_And_No_NameIdentifier_User_Status_Returns_Null_ViewData(
        Guid candidateId,
        [ArrangeActionContext<ServiceController>] ActionExecutingContext context,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        [Frozen] Mock<IApiClient> apiClient,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(x => x.RequestServices.GetService(typeof(IApiClient)))
            .Returns(apiClient.Object);
        httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, candidateId.ToString())
        })));

        context.ActionDescriptor = new ControllerActionDescriptor
        {
            MethodInfo = typeof(DummyExemptionClass).GetMethod(nameof(DummyExemptionClass.NoExemption))!,
        };
        context.HttpContext = httpContext.Object;
        var request = new PutCandidateApiRequest(It.IsAny<string>(), new PutCandidateApiRequestData
        {
            Email = It.IsAny<string>()
        });

        //Act
        await filter.OnActionExecutionAsync(context, nextMethod.Object);
        ((Controller)context.Controller).ViewData["IsAccountStatusCompleted"].Should().BeNull();
        apiClient.Verify(x => x.Put<PutCandidateApiResponse>(
            It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(request.PutUrl)
                                               && ((PutCandidateApiRequestData)c.Data).Email == It.IsAny<string>())), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_Account_Incomplete_Then_Redirected(
        Guid candidateId,
        [ArrangeActionContext<UserController>] ActionExecutingContext contextController,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        [Frozen] Mock<IApiClient> apiClient,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(x => x.RequestServices.GetService(typeof(IApiClient)))
            .Returns(apiClient.Object);
        httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaims.CandidateId, candidateId.ToString())
        })));

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.Setup(x => x.Path).Returns(() => new PathString("/some-url"));
        httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

        contextController.ActionDescriptor = new ControllerActionDescriptor
        {
            MethodInfo = typeof(DummyExemptionClass).GetMethod(nameof(DummyExemptionClass.NoExemption))!
        };
        contextController.HttpContext = httpContext.Object;

        //Act
        await filter.OnActionExecutionAsync(contextController, nextMethod.Object);

        //Assert
        nextMethod.Verify(x => x(), Times.Never);
        contextController.Result.Should().BeOfType<RedirectToRouteResult>();
        var redirectToRouteResult = contextController.Result as RedirectToRouteResult;
        redirectToRouteResult!.RouteName.Should().Be(RouteNames.CreateAccount);
    }

    [Test, MoqAutoData]
    public async Task And_Account_Has_Already_Been_Migrated_And_Target_Is_Exempt_Then_Continues(
        Guid candidateId,
        [ArrangeActionContext<UserController>] ActionExecutingContext contextController,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        contextController.ActionDescriptor = new ControllerActionDescriptor
        {
            MethodInfo = typeof(DummyExemptionClass).GetMethod(nameof(DummyExemptionClass.AllowMigratedAccountAccess))!,
        };

        contextController.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(CustomClaims.AccountSetupCompleted, "true"),
        }));

        //Act
        await filter.OnActionExecutionAsync(contextController, nextMethod.Object);

        //Assert
        nextMethod.Verify(x => x(), Times.Once);
        Assert.That(contextController.Result, Is.Null);
    }

    private class DummyExemptionClass
    {
        public void NoExemption()
        {
        }

        [AllowIncompleteAccountAccess]
        public void AllowIncompleteAccountAccess()
        {
        }

        [AllowMigratedAccountAccess]
        public void AllowMigratedAccountAccess()
        {
        }
    }
}