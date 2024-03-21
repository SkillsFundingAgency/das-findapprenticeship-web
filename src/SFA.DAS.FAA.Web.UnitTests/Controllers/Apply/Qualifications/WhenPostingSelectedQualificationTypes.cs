using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenPostingSelectedQualificationTypes
{
    [Test, MoqAutoData]
    public async Task Then_If_Nothing_Selected_Then_Error_Returned(
        AddQualificationSelectTypeViewModel model,
        GetQualificationTypesQueryResponse queryResult,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        model.QualificationReferenceId = Guid.Empty;
        mediator.Setup(x => x.Send(It.Is<GetQualificationTypesQuery>(c=>c.ApplicationId == model.ApplicationId && c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        var actual = await controller.AddQualificationSelectTypePost(model) as ViewResult;

        controller.ModelState["QualificationReferenceId"]!.Errors.First().ErrorMessage.Should().Be("Select your most recent qualification");
        Assert.That(actual, Is.Not.Null);
        actual!.ViewName.Should().Be("~/Views/apply/Qualifications/AddQualificationSelectType.cshtml");
        var actualModel = actual.Model as AddQualificationSelectTypeViewModel;
        actualModel!.ApplicationId.Should().Be(model.ApplicationId);
        actualModel.Qualifications.Should().BeEquivalentTo(queryResult.QualificationTypes.Select(c=>new AddQualificationSelectTypeViewModel.QualificationType
        {
            QualificationDisplayTypeViewModel = new QualificationDisplayTypeViewModel(c.Name),
            Id = c.Id,
            Name = c.Name
        }).ToList());
    }
    [Test, MoqAutoData]
    public async Task Then_If_Value_Selected_Then_Redirected(
        AddQualificationSelectTypeViewModel model,
        GetQualificationTypesQueryResponse queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetQualificationTypesQuery>(c=>c.ApplicationId == model.ApplicationId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.AddQualificationSelectTypePost(model) as RedirectToRouteResult;

        Assert.That(actual, Is.Not.Null);
        actual!.RouteName.Should().Be(RouteNames.ApplyApprenticeship.AddQualification);
        actual!.RouteValues!["ApplicationId"].Should().Be(model.ApplicationId);
        actual!.RouteValues!["QualificationReferenceId"].Should().Be(model.QualificationReferenceId);
        
    }
}