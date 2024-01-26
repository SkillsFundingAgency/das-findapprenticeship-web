using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    [TestFixture]
    public class WhenGettingRequest
    {
        [Test, MoqAutoData]
        public void Then_View_Returned(
            [Frozen] Mock<IMediator> mediator)
        {
            //arrange
            var request = new AddWorkHistoryRequest
            {
                ApplicationId = Guid.NewGuid(),
                AddJob = "Yes",
            };

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            var controller = new WorkHistoryController(mediator.Object)
            {
                Url = mockUrlHelper.Object
            };

            var actual = controller.Get(request) as ViewResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();

            var actualModel = actual?.Model as AddWorkHistoryRequest;
            actualModel?.ApplicationId.Should().Be(request.ApplicationId);
            actualModel?.AddJob.Should().Be(request.AddJob);
        }

        [Test, MoqAutoData]
        public void Then_Request_With_ValidationError_Is_Called_And_View_Returned(
            AddWorkHistoryRequest request,
            [Frozen] Mock<IMediator> mediator)
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            var controller = new WorkHistoryController(mediator.Object)
            {
                Url = mockUrlHelper.Object
            };

            controller.ModelState.AddModelError("SomeProperty", "SomeError");
            
            var actual = controller.Get(request) as ViewResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            var actualModel = actual?.Model as AddWorkHistoryRequest; 
            actualModel?.ErrorDictionary.Should().NotBeNull();
        }
    }
}
