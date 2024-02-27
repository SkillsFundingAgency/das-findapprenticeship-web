using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.UpdateVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience.EditVolunteeringAndWorkExperience;

[TestFixture]
public class WhenPostingEditVolunteeringAndWorkExperienceRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid candidateId,
        EditVolunteeringAndWorkExperienceViewModel request,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.Apply.VolunteeringAndWorkExperienceController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<UpdateVolunteeringAndWorkExperienceCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)
                && c.CandidateId.Equals(candidateId)
                && c.CompanyName!.Equals(request.CompanyName)
                && c.StartDate.Equals(request.StartDate)
                && c.EndDate.Equals(request.EndDate)
                && c.Description!.Equals(request.Description)
            ), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);

        var actual = await controller.PostEditVolunteeringAndWorkExperience(request) as RedirectToRouteResult;
        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary);
    }
}