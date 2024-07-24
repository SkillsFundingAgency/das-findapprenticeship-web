using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience;
public class WhenPostingAddVolunteeringAndWorkExperiencePage
{
    [Test, MoqAutoData]
    public async Task And_User_Has_No_Working_Experiences_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        GetVolunteeringAndWorkExperiencesQueryResult queryResult,
        UpdateVolunteeringAndWorkExperienceApplicationCommandResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        var request = new VolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyVolunteeringOrWorkExperience = false,
        };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        mediator.Setup(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(c =>
                c.ApplicationId.Equals(applicationId)
                && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.Post(request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.Apply);
            actual?.RouteValues.Should().NotBeEmpty();
            mediator.Verify(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task And_User_Has_Training_Courses_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        UpdateVolunteeringAndWorkExperienceApplicationCommandResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        var request = new VolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyVolunteeringOrWorkExperience = true,
        };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        mediator.Setup(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Post(request) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience);
            mediator.Verify(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
