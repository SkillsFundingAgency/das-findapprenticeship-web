using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpsertQualification;
using SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenPostingAddQualification
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Called_And_Redirected_With_Empty_Subjects_Ignored(
        Guid candidateId,
        SubjectViewModel subject,
        AddQualificationViewModel model,
        Mock<IValidator<AddQualificationViewModel>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        // arrange
        model.IsApprenticeship = false;
        model.Subjects = [subject, new SubjectViewModel()];
        controller
            .AddControllerContext()
            .WithUser(candidateId);
        validator
            .Setup(x => x.ValidateAsync(It.Is<AddQualificationViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        // act
        var actual = await controller.ModifyQualification(validator.Object, model) as RedirectToRouteResult;

        // assert
        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.Qualifications);
        mediator.Verify(x=>x.Send(It.Is<UpsertQualificationCommand>(
                c=>c.CandidateId == candidateId && c.Subjects.Count == 1)
            , CancellationToken.None), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Model_Error_View_Returned_And_Command_Not_Called(
        string id,
        string title,
        Guid candidateId,
        SubjectViewModel data,
        AddQualificationViewModel model,
        Mock<IValidator<AddQualificationViewModel>> validator,
        GetModifyQualificationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        // arrange
        controller
            .AddControllerContext()
            .WithUser(candidateId);
        controller.ControllerContext.ModelState.AddModelError("error","error");
        queryResult.QualificationType!.Name = "BTec";
        mediator.Setup(x => x.Send(It.Is<GetModifyQualificationQuery>(c =>
                c.QualificationReferenceId == model.QualificationReferenceId
                && c.CandidateId == candidateId
                && c.ApplicationId == model.ApplicationId), CancellationToken.None))
            .ReturnsAsync(queryResult);
        
        validator
            .Setup(x => x.ValidateAsync(It.Is<AddQualificationViewModel>(m => m == model), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        
        // act
        var actual = await controller.ModifyQualification(validator.Object, model) as ViewResult;

        // assert
        Assert.That(actual, Is.Not.Null);
        actual!.ViewName.Should().Be("~/Views/apply/Qualifications/AddQualification.cshtml");
        var actualModel = actual.Model as AddQualificationViewModel;
        actualModel!.ApplicationId.Should().Be(model.ApplicationId);
        actualModel.QualificationDisplayTypeViewModel!.Title.Should().Contain("BTEC");
        mediator.Verify(x=>x.Send(It.IsAny<UpsertQualificationCommand>()
            , CancellationToken.None), Times.Never);
    }
}