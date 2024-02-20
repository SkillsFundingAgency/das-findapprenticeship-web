using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience;
public class WhenPostingDeleteVolunteeringOrWorkExperience
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Sent_Successfully(
        Guid candidateId,
        DeleteVolunteeringOrWorkExperienceViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        await controller.PostDelete(model);


        mediator.Verify(x => x.Send(It.Is<DeleteVolunteeringOrWorkExperienceCommand>(c =>
            c.CandidateId == candidateId
            && c.ApplicationId == model.ApplicationId
            && c.Id == model.Id),
        CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_An_Exception_Is_Thrown_Then_ModelState_Is_Updated_And_View_Returned(
        Guid candidateId,
        DeleteVolunteeringOrWorkExperienceViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.IsAny<DeleteVolunteeringOrWorkExperienceCommand>(),
                CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException());

        var result = await controller.PostDelete(model) as ViewResult;

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.ViewName.Should().Be("~/Views/apply/volunteeringandworkexperience/DeleteVolunteeringOrWorkExperience.cshtml");
            controller.ModelState.ContainsKey(nameof(DeleteVolunteeringOrWorkExperienceViewModel)).Should().BeTrue();
            controller.ModelState[nameof(DeleteVolunteeringOrWorkExperienceViewModel)].Errors.Should().Contain(e => e.ErrorMessage == "There's been a problem");
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Exception_Is_Thrown_Then_RedirectToRouteResult_Is_Returned(
        Guid candidateId,
        DeleteVolunteeringOrWorkExperienceViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        var result = await controller.PostDelete(model) as RedirectToRouteResult;

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience);
    }
}
