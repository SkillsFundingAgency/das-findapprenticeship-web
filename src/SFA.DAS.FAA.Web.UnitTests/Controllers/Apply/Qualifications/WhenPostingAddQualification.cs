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
    public async Task Then_The_Command_Is_Called_And_Redirected(
        Guid candidateId,
        AddQualificationViewModel model,
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
        
        var actual = await controller.ModifyQualification(model) as RedirectToRouteResult;

        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.Qualifications);
        mediator.Verify(x=>x.Send(It.Is<UpsertQualificationCommand>(
                c=>c.CandidateId == candidateId)
            , CancellationToken.None), Times.Once);
    }
}