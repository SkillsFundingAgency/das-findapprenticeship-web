using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Applications.Withdraw;
using SFA.DAS.FAA.Application.Queries.Applications.Withdraw;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Applications;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Applications;

public class WhenPostingWithdrawApplication
{
    [Test, MoqAutoData]
    public async Task Then_If_Valid_And_Yes_Command_Sent_And_Cached_Message_Stored(
        PostWithdrawApplicationViewModel model,
        Guid candidateId,
        string govIdentifier,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController controller)
    {
        model.WithdrawApplication = true;
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(ClaimTypes.NameIdentifier, govIdentifier)
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var actual = await controller.Withdraw(model) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        actual.RouteValues["tab"].Should().Be(ApplicationsTab.Submitted);
        mediator.Verify(x => x.Send(It.Is<WithdrawApplicationCommand>(c =>
            c.CandidateId.Equals(candidateId) && c.ApplicationId.Equals(model.ApplicationId)), It.IsAny<CancellationToken>()), Times.Once);
        cacheStorageService.Verify(x=>x.Set($"{govIdentifier}-VacancyWithdrawn", $"Application withdrawn for {model.AdvertTitle} at {model.EmployerName}.", 1, 1), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Valid_And_No_Command_Not_Sent_And_Cached_Message_Not_Stored(
        PostWithdrawApplicationViewModel model,
        Guid candidateId,
        string govIdentifier,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController controller)
    {
        model.WithdrawApplication = false;
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(ClaimTypes.NameIdentifier, govIdentifier)
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        var actual = await controller.Withdraw(model) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual.RouteName.Should().Be(RouteNames.Applications.ViewApplications);
        actual.RouteValues["tab"].Should().Be(ApplicationsTab.Submitted);
        mediator.Verify(x => x.Send(It.IsAny<WithdrawApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        cacheStorageService.Verify(x=>x.Set(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Valid_View_Returned(
        PostWithdrawApplicationViewModel model,
        Guid candidateId,
        string govIdentifier,
        GetWithdrawApplicationQueryResult result,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetWithdrawApplicationQuery>(c =>
                c.CandidateId.Equals(candidateId) && c.ApplicationId.Equals(model.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaims.CandidateId, candidateId.ToString()),
            new(ClaimTypes.NameIdentifier, govIdentifier)
        }));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        controller.ModelState.AddModelError("Error","Error");
        
        var actual = await controller.Withdraw(model) as ViewResult;
        
        actual.Should().NotBeNull();
        var actualModel = actual.Model as WithdrawApplicationViewModel;
        actualModel.ApplicationId.Should().Be(result.ApplicationId);
    }
}