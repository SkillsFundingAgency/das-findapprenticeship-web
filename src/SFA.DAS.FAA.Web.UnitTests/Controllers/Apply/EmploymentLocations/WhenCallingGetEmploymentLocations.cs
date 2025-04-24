using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EmploymentLocations;

[TestFixture]
public class WhenCallingGetEmploymentLocations
{
    [Test, MoqAutoData]
    public async Task Then_View_Is_Returned(
        Guid applicationId,
        Guid candidateId,
        GetEmploymentLocationsQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator)
    {
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        mediator.Setup(x => x.Send(It.Is<GetEmploymentLocationsQuery>(q => q.ApplicationId == applicationId && q.CandidateId == candidateId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controller = new EmploymentLocationsController(mediator.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            }
        };

        var actual = await controller.AddEmploymentLocations(applicationId) as ViewResult;
        var actualModel = actual!.Model.As<AddEmploymentLocationsViewModel>();

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Model.Should().NotBeNull();
            actualModel.Addresses.Should().BeEquivalentTo(queryResult.EmploymentLocation.Addresses);
        }
    }
}