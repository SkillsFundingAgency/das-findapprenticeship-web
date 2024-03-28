using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpsertQualification;
using SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenPostingAddQualification
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Called_And_Redirected_With_Empty_Subjects_Ignored(
        Guid candidateId,
        SubjectViewModel subject,
        AddQualificationViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        model.IsApprenticeship = false;
        model.Subjects = [subject, new SubjectViewModel()];
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        
        var actual = await controller.ModifyQualification(model) as RedirectToRouteResult;

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
        GetModifyQualificationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };
        controller.ControllerContext.ModelState.AddModelError("error","error");
        queryResult.QualificationType!.Name = "BTec";
        mediator.Setup(x => x.Send(It.Is<GetModifyQualificationQuery>(c =>
                c.QualificationReferenceId == model.QualificationReferenceId
                && c.CandidateId == candidateId
                && c.ApplicationId == model.ApplicationId), CancellationToken.None))
            .ReturnsAsync(queryResult);
        
        var actual = await controller.ModifyQualification(model) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        actual!.ViewName.Should().Be("~/Views/apply/Qualifications/AddQualification.cshtml");
        var actualModel = actual.Model as AddQualificationViewModel;
        actualModel!.ApplicationId.Should().Be(model.ApplicationId);
        actualModel.QualificationDisplayTypeViewModel!.Title.Should().Contain("BTEC");
        mediator.Verify(x=>x.Send(It.IsAny<UpsertQualificationCommand>()
            , CancellationToken.None), Times.Never);
    }
}