using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteQualifications;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.Qualifications;

public class WhenGettingDeleteQualifications
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned_And_QualificationsAdded_To_ViewModel(
        Guid applicationId,
        Guid candidateId,
        Guid qualificationType,
        GetDeleteQualificationsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationsQuery>(c=>c.ApplicationId == applicationId && c.QualificationType == qualificationType && c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync(queryResult);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        var actual = await controller.DeleteQualifications(applicationId, qualificationType) as ViewResult;

        actual.Should().NotBeNull();

        actual!.ViewName.Should().Be("~/Views/apply/Qualifications/DeleteQualifications.cshtml");
        var actualModel = actual.Model as DeleteQualificationsViewModel;
        actualModel!.ApplicationId.Should().Be(applicationId);
        actualModel!.Qualifications.Should().BeEquivalentTo(queryResult.Qualifications);
    }

    [Test, MoqAutoData]
    public async Task Then_Response_Is_Null_Returned_Redirect(
        Guid applicationId,
        Guid candidateId,
        Guid qualificationType,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] QualificationsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationsQuery>(c => c.ApplicationId == applicationId && c.QualificationType == qualificationType && c.CandidateId == candidateId), CancellationToken.None))
            .ReturnsAsync((GetDeleteQualificationsQueryResult)null!);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        var actual = await controller.DeleteQualifications(applicationId, qualificationType) as RedirectToRouteResult;

        actual.Should().NotBeNull();
        actual!.RouteName.Should().Be(RouteNames.Apply);
    }
}