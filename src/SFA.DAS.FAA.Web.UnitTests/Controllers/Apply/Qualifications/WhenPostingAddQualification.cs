using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.UpsertQualification;
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
    public async Task Then_The_Command_Is_Called_And_Apprenticeship_Saved_If_Passed(
        string id,
        string title,
        Guid candidateId,
        SubjectViewModel data,
        AddQualificationViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        model.IsApprenticeship = true;
        data.AdditionalInformation = null;
        data.Level = null;
        data.Name = $"{id}|{title}";
        model.Subjects = [data];
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
                c=>c.CandidateId == candidateId
                && c.Subjects.First().Name == title
                && c.Subjects.First().AdditionalInformation == id)
            , CancellationToken.None), Times.Once);
    }
}