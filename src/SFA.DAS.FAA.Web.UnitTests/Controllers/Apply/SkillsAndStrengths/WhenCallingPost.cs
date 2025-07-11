﻿using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.SkillsAndStrengths;
public class WhenCallingPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        GetExpectedSkillsAndStrengthsQueryResult expectedSkills,
        UpdateSkillsAndStrengthsCommandResult createSkillsAndStrengthsCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SkillsAndStrengthsController controller)
    {
        var request = new SkillsAndStrengthsViewModel(expectedSkills, applicationId)
        {
            ApplicationId = Guid.NewGuid(),
            IsSectionComplete = true
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<UpdateSkillsAndStrengthsCommand>(c =>
        c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createSkillsAndStrengthsCommandResult);

        var actual = await controller.Post(request.ApplicationId, request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.RouteName.Should().BeEquivalentTo(RouteNames.Apply);
        }
    }
    
    [Test, MoqAutoData]
    public async Task And_Autosaving_JsonResult_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        GetExpectedSkillsAndStrengthsQueryResult expectedSkills,
        UpdateSkillsAndStrengthsCommandResult createSkillsAndStrengthsCommandResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SkillsAndStrengthsController controller)
    {
        // arrange
        var request = new SkillsAndStrengthsViewModel(expectedSkills, applicationId)
        {
            ApplicationId = Guid.NewGuid(),
            IsSectionComplete = true,
            AutoSave = true
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<UpdateSkillsAndStrengthsCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createSkillsAndStrengthsCommandResult);

        // act
        var actual = await controller.Post(request.ApplicationId, request) as JsonResult;

        // assert
        actual.Should().NotBeNull();
    }
}
