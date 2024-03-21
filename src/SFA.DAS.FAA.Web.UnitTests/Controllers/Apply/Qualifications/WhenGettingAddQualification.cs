using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenGettingAddQualification
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Called_And_View_Displayed(
        Guid applicationId,
        Guid qualificationReferenceId,
        Guid candidateId,
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
        queryResult.QualificationType!.Name = "BTec";
        mediator.Setup(x => x.Send(It.Is<GetModifyQualificationQuery>(c=>
                c.QualificationReferenceId == qualificationReferenceId
                && c.CandidateId == candidateId
                && c.ApplicationId == applicationId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.ModifyQualification(applicationId, qualificationReferenceId) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        actual!.ViewName.Should().Be("~/Views/apply/Qualifications/AddQualification.cshtml");
        var actualModel = actual.Model as AddQualificationViewModel;
        actualModel!.ApplicationId.Should().Be(applicationId);
        actualModel.QualificationDisplayTypeViewModel.Title.Should().Contain("BTEC");
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Qualification_Type_Is_Returned_Then_Redirect_To_Select_Qualification(
        Guid applicationId,
        Guid qualificationReferenceId,
        Guid candidateId,
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
        queryResult.QualificationType = null;
        mediator.Setup(x => x.Send(It.Is<GetModifyQualificationQuery>(c=>c.QualificationReferenceId == qualificationReferenceId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.ModifyQualification(applicationId, qualificationReferenceId) as RedirectToRouteResult;

        Assert.That(actual, Is.Not.Null);
        actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.AddQualificationSelectType);
        actual.RouteValues["applicationId"].Should().Be(applicationId);
    }
}