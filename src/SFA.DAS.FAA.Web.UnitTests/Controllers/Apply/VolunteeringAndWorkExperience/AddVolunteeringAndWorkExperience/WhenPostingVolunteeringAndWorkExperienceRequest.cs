using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience.AddVolunteeringAndWorkExperience;

[TestFixture]
public class WhenPostingVolunteeringAndWorkExperienceRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid candidateId,
        AddVolunteeringAndWorkExperienceViewModel request,
        AddVolunteeringAndWorkExperienceCommandResult result,
        Mock<IValidator<AddVolunteeringAndWorkExperienceViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.Apply.VolunteeringAndWorkExperienceController controller)
    {
        // arrange
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<AddVolunteeringAndWorkExperienceCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)
                && c.CandidateId.Equals(candidateId)
                && c.CompanyName!.Equals(request.CompanyName)
                && c.StartDate.Equals(request.StartDate)
                && c.EndDate.Equals(request.EndDate)
                && c.Description!.Equals(request.Description)
            ), It.IsAny<CancellationToken>()))

            .ReturnsAsync(result);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<AddVolunteeringAndWorkExperienceViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.PostAddVolunteeringAndWorkExperience(validator.Object, request) as RedirectToRouteResult;
        
        // assert
        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary);
    }
}