using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses;
public class WhenGettingRequest
{
    [Test, MoqAutoData]
    public async Task Then_View_Returned(
        Guid applicationId,
        Guid candidateId,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
        .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
        .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetTrainingCoursesQuery>(q =>
            q.ApplicationId == applicationId
            && q.CandidateId == candidateId)
            , It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetTrainingCoursesQueryResult { TrainingCourses = [] });

        var controller = new TrainingCoursesController(mediator.Object)
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

        var actual = await controller.Get(applicationId) as ViewResult;
        var actualModel = actual?.Model as TrainingCoursesViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.ApplicationId.Should().Be(applicationId);
        }
    }
}
