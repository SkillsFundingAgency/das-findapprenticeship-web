using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringAndWorkExperiences;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.VolunteeringAndWorkExperience.SummaryVolunteeringAndWorkExperience;

[TestFixture]
public class WhenGettingSummaryVolunteeringAndWorkExperiencePage
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid candidateId,
        Guid applicationId,
        GetVolunteeringAndWorkExperiencesQueryResult result,
        List<GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience> workExperiences,
        [Frozen] Mock<IMediator> mediator)
    {
        //arrange
        result.VolunteeringAndWorkExperiences = workExperiences;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new VolunteeringAndWorkExperienceController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(c =>
                c.ApplicationId.Equals(applicationId)
                && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Summary(applicationId) as ViewResult;
        actual.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_Redirect_Route_Returned_When_Result_Is_Null(
        Guid candidateId,
        Guid applicationId,
        GetVolunteeringAndWorkExperiencesQueryResult result,
        [Frozen] Mock<IMediator> mediator)
    {
        //arrange
        result.VolunteeringAndWorkExperiences = new List<GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new VolunteeringAndWorkExperienceController(mediator.Object)
        {
            Url = mockUrlHelper.Object
        };
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    { new(CustomClaims.CandidateId, candidateId.ToString()) }))
            }
        };

        mediator.Setup(x => x.Send(It.Is<GetVolunteeringAndWorkExperiencesQuery>(c =>
                c.ApplicationId.Equals(applicationId)
                && c.CandidateId.Equals(candidateId)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Summary(applicationId) as RedirectToRouteResult;
        actual!.RouteName.Should().NotBeNull();
        actual.RouteName.Should().Be(RouteNames.ApplyApprenticeship.VolunteeringAndWorkExperience);
    }
}