using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.VolunteeringAndWorkExperience.UpdateVolunteeringAndWorkExperience;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience.EditVolunteeringAndWorkExperience;

[TestFixture]
public class WhenPostingEditVolunteeringAndWorkExperienceRequest
{
    [Test, MoqAutoData]
    public async Task Then_RedirectRoute_Returned(
        Guid candidateId,
        EditVolunteeringAndWorkExperienceViewModel request,
        Mock<IValidator<EditVolunteeringAndWorkExperienceViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        // arrange
        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<UpdateVolunteeringAndWorkExperienceCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)
                && c.CandidateId.Equals(candidateId)
                && c.CompanyName!.Equals(request.CompanyName)
                && c.StartDate.Equals(request.StartDate)
                && c.EndDate.Equals(request.EndDate)
                && c.Description!.Equals(request.Description)
            ), It.IsAny<CancellationToken>()))
            .Returns(() => Task.CompletedTask);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<EditVolunteeringAndWorkExperienceViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.PostEditVolunteeringAndWorkExperience(validator.Object, request) as RedirectToRouteResult;
        
        // assert
        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperienceSummary);
    }
}