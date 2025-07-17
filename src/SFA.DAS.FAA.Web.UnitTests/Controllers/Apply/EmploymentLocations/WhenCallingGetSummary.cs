using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using System.Security.Claims;
using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EmploymentLocations
{
    [TestFixture]
    public class WhenCallingGetSummary
    {
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            Address mockAddress,
            bool isEdit,
            Guid applicationId,
            Guid candidateId,
            GetEmploymentLocationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator)
        {
            foreach (var employmentLocationAddress in queryResult.EmploymentLocation.Addresses)
            {
                employmentLocationAddress.IsSelected = true;
                employmentLocationAddress.FullAddress = JsonConvert.SerializeObject(mockAddress);
            }
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

            var actual = await controller.Summary(applicationId, isEdit) as ViewResult;
            var actualModel = actual!.Model.As<EmploymentLocationsSummaryViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.Addresses.Should().BeEquivalentTo(queryResult.EmploymentLocation.Addresses, options => options.Excluding(c => c.FullAddress));
                actualModel.ApplicationId.Should().Be(applicationId);
                actualModel.IsSectionCompleted.Should().Be(queryResult.IsSectionCompleted);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_Null_Address_View_Is_Returned(
            bool isEdit,
            Guid applicationId,
            Guid candidateId,
            GetEmploymentLocationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator)
        {
            foreach (var employmentLocationAddress in queryResult.EmploymentLocation.Addresses)
            {
                employmentLocationAddress.IsSelected = true;
                employmentLocationAddress.FullAddress = string.Empty;
            }
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

            var actual = await controller.Summary(applicationId, isEdit) as ViewResult;
            var actualModel = actual!.Model.As<EmploymentLocationsSummaryViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.Addresses.Should().BeEquivalentTo(queryResult.EmploymentLocation.Addresses, options => options.Excluding(c => c.FullAddress));
                actualModel.ApplicationId.Should().Be(applicationId);
                actualModel.IsSectionCompleted.Should().Be(queryResult.IsSectionCompleted);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_Empty_Address_View_Is_Returned(
            bool isEdit,
            Guid applicationId,
            Guid candidateId,
            GetEmploymentLocationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator)
        {
            foreach (var employmentLocationAddress in queryResult.EmploymentLocation.Addresses)
            {
                employmentLocationAddress.IsSelected = true;
                employmentLocationAddress.FullAddress = JsonConvert.SerializeObject(new Address("", "", "", "", ""));
            }
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

            var actual = await controller.Summary(applicationId, isEdit) as ViewResult;
            var actualModel = actual!.Model.As<EmploymentLocationsSummaryViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.Addresses.Should().BeEquivalentTo(queryResult.EmploymentLocation.Addresses, options => options.Excluding(c => c.FullAddress));
                actualModel.ApplicationId.Should().Be(applicationId);
                actualModel.IsSectionCompleted.Should().Be(queryResult.IsSectionCompleted);
            }
        }
    }
}
