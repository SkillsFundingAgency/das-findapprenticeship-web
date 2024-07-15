using System.Security.Claims;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Filters;
using SFA.DAS.FAA.Web.UnitTests.Customisations;
using SFA.DAS.Testing.AutoFixture;

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
    
    [Test, MoqAutoData]
    public async Task And_Has_Candidate_And_No_Display_Name_And_UserController_The_Continues(
        Guid candidateId,
        [ArrangeActionContext<UserController>] ActionExecutingContext contextController,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        contextController.ActionDescriptor = new ActionDescriptor
        {
            DisplayName = nameof(UserController)
        };
        contextController.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaims.CandidateId, candidateId.ToString())
        }));
            
        //Act
        await filter.OnActionExecutionAsync(contextController, nextMethod.Object);

        //Assert
        nextMethod.Verify( x => x(), Times.Once);
        Assert.That(contextController.Result ,Is.Null);
    }

    [Test, MoqAutoData]
    public async Task And_Has_Candidate_And_No_Display_Name_And_HomeController_The_Continues(
        Guid candidateId,
        [ArrangeActionContext<HomeController>] ActionExecutingContext contextController,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        contextController.ActionDescriptor = new ActionDescriptor
        {
            DisplayName = nameof(HomeController)
        };
        contextController.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaims.CandidateId, candidateId.ToString())
        }));

        //Act
        await filter.OnActionExecutionAsync(contextController, nextMethod.Object);

        //Assert
        nextMethod.Verify(x => x(), Times.Once);
        Assert.That(contextController.Result, Is.Null);
    }

    [Test, MoqAutoData]
    public async Task And_Has_Candidate_And_No_Display_Name_And_ServiceController_The_Continues(
        Guid candidateId,
        [ArrangeActionContext<ServiceController>] ActionExecutingContext contextController,
        [Frozen] Mock<ActionExecutionDelegate> nextMethod,
        NewFaaUserAccountFilter filter)
    {
        //Arrange
        contextController.ActionDescriptor = new ActionDescriptor
        {
            DisplayName = nameof(ServiceController)
        };
        contextController.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaims.CandidateId, candidateId.ToString())
        }));

        //Act
        await filter.OnActionExecutionAsync(contextController, nextMethod.Object);

        //Assert
        nextMethod.Verify(x => x(), Times.Once);
        Assert.That(contextController.Result, Is.Null);
    }

}