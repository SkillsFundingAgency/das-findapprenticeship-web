using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.SkillsAndStrengths;
using SFA.DAS.FAA.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.SkillsAndStrengths;

public class WhenCallingPost
{
    [Test, MoqAutoData]
    public async Task And_ModelState_Is_Valid_Then_Redirected_To_TaskList(
        Guid candidateId,
        Guid applicationId,
        GetExpectedSkillsAndStrengthsQueryResult expectedSkills,
        UpdateSkillsAndStrengthsCommandResult createSkillsAndStrengthsCommandResult,
        Mock<IValidator<SkillsAndStrengthsViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SkillsAndStrengthsController controller)
    {
        // arrange
        var request = new SkillsAndStrengthsViewModel(expectedSkills, applicationId)
        {
            ApplicationId = Guid.NewGuid(),
            IsSectionComplete = true
        };

        controller.WithContext(x => x.WithUser(candidateId));

        mediator.Setup(x => x.Send(It.Is<UpdateSkillsAndStrengthsCommand>(c =>
        c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createSkillsAndStrengthsCommandResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<SkillsAndStrengthsViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, request.ApplicationId, request) as RedirectToRouteResult;

        // assert
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
        Mock<IValidator<SkillsAndStrengthsViewModel>> validator,
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

        controller.WithContext(x => x.WithUser(candidateId));
        
        mediator.Setup(x => x.Send(It.Is<UpdateSkillsAndStrengthsCommand>(c =>
                c.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createSkillsAndStrengthsCommandResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<SkillsAndStrengthsViewModel>(m => m == request), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Post(validator.Object, request.ApplicationId, request) as JsonResult;

        // assert
        actual.Should().NotBeNull();
    }
}