using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication.VolunteeringAndWorkExperience;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience;

public class WhenPostingAddVolunteeringAndWorkExperiencePage
{
    [Test, MoqAutoData]
    public async Task And_User_Has_No_Working_Experiences_Then_Mediator_Is_Called_And_Redirect_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        GetVolunteeringAndWorkExperiencesQueryResult queryResult,
        UpdateVolunteeringAndWorkExperienceApplicationCommandResult result,
        Mock<IValidator<VolunteeringAndWorkExperienceViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        // arrange
        var request = new VolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyVolunteeringOrWorkExperience = false,
        };
        controller.WithContext(x => x.WithUser(candidateId));
        mediator.Setup(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(c =>
                c.ApplicationId.Equals(applicationId)
                && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);
        validator
            .Setup(x => x.ValidateAsync(It.Is<VolunteeringAndWorkExperienceViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, request) as RedirectToRouteResult;

        // assert
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
        Mock<IValidator<VolunteeringAndWorkExperienceViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VolunteeringAndWorkExperienceController controller)
    {
        // arrange
        var request = new VolunteeringAndWorkExperienceViewModel
        {
            ApplicationId = applicationId,
            DoYouWantToAddAnyVolunteeringOrWorkExperience = true,
        };
        
        controller.WithContext(x => x.WithUser(candidateId));
        mediator.Setup(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        validator
            .Setup(x => x.ValidateAsync(It.Is<VolunteeringAndWorkExperienceViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, request) as RedirectToRouteResult;

        // assert
        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.ApplyApprenticeship.AddVolunteeringAndWorkExperience);
            mediator.Verify(x => x.Send(It.IsAny<UpdateVolunteeringAndWorkExperienceApplicationCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}