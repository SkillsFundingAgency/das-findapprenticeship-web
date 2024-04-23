using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.DeleteQualifications;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenPostingDeleteQualifications
{
    [Test, MoqAutoData]
    public async Task Then_User_Is_Redirected_To_Qualifications_Summary(
        Guid candidateId,
        DeleteQualificationsViewModel viewModel,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<DeleteQualificationsCommand>(c=>c.ApplicationId == viewModel.ApplicationId && c.QualificationReferenceId == viewModel.QualificationReferenceId && c.CandidateId == candidateId), CancellationToken.None))
            .Returns(Task.CompletedTask);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        var actual = await controller.DeleteQualifications(viewModel) as RedirectToRouteResult;

        actual.Should().NotBeNull();

        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.Qualifications);
    }
}