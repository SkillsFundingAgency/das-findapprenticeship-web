using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.FAA.Domain.Candidates;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Attributes;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Filters;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.UnitTests.Customisations;
using System.Security.Claims;
using SFA.DAS.FAA.Domain.Enums;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

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
    [MoqInlineAutoData(UserStatus.InProgress, false)]
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


    [Test]
    [MoqInlineAutoData(10, 20, "30")]
    [MoqInlineAutoData(0, 0, "0")]
    [MoqInlineAutoData(99, 1, "99+")]
    [MoqInlineAutoData(100, 0, "99+")]
    public async Task And_Has_Application_Notification_Count_Returns_ViewData(
        int successCount,
        int unSuccessCount,
        string expectedViewData,
        UserStatus status,
        string email,
        string userId,
        Guid candidateId,
        PutCandidateApiResponse response,
        [ArrangeActionContext<SearchApprenticeshipsController>] ActionExecutingContext context,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        [Frozen] Mock<IApiClient> apiClient,
        [Frozen] Mock<INotificationCountService> notificationCountService,
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
        httpContext.Setup(x => x.RequestServices.GetService(typeof(INotificationCountService)))
            .Returns(notificationCountService.Object);

        httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(CustomClaims.CandidateId, candidateId.ToString()),
            new Claim(CustomClaims.AccountSetupCompleted, "true"),
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

        notificationCountService.Setup(x => x.GetUnreadApplicationCount(candidateId, ApplicationStatus.Successful))
            .ReturnsAsync(successCount);
        notificationCountService.Setup(x => x.GetUnreadApplicationCount(candidateId, ApplicationStatus.Unsuccessful))
            .ReturnsAsync(unSuccessCount);

        context.HttpContext = httpContext.Object;

        //Act
        await filter.OnActionExecutionAsync(context, nextMethod.Object);
        ((Controller)context.Controller).ViewData[ViewDataKeys.ApplicationsCount].Should().NotBeNull();
        ((Controller)context.Controller).ViewData[ViewDataKeys.ApplicationsCount].Should().Be(expectedViewData);
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

    [Test]
    [MoqInlineAutoData(UserStatus.Incomplete)]
    [MoqInlineAutoData(UserStatus.InProgress)]
    public async Task And_Account_Incomplete_or_InProgress_Then_Redirected(
        UserStatus status,
        Guid candidateId,
        string email,
        string userId,
        PutCandidateApiResponse response,
        [ArrangeActionContext<UserController>] ActionExecutingContext contextController,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        [Frozen] Mock<IApiClient> apiClient,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        response.Status = status;
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(x => x.RequestServices.GetService(typeof(IApiClient)))
            .Returns(apiClient.Object);
        httpContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.NameIdentifier, userId)
        })));

        var httpRequest = new Mock<HttpRequest>();
        httpRequest.Setup(x => x.Path).Returns(() => new PathString("/some-url"));
        httpContext.Setup(x => x.Request).Returns(httpRequest.Object);

        contextController.ActionDescriptor = new ControllerActionDescriptor
        {
            MethodInfo = typeof(DummyExemptionClass).GetMethod(nameof(DummyExemptionClass.NoExemption))!
        };
        contextController.HttpContext = httpContext.Object;

        var request = new PutCandidateApiRequest(userId, new PutCandidateApiRequestData
        {
            Email = email
        });
        apiClient.Setup(x =>
            x.Put<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(c => c.PutUrl.Equals(request.PutUrl)
                                                   && ((PutCandidateApiRequestData)c.Data).Email == email
                ))).ReturnsAsync(response);

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