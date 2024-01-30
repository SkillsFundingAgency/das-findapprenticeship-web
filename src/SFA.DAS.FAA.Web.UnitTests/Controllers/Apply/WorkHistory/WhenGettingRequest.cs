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
            Guid applicationId,
            [Frozen] Mock<IMediator> mediator)
        {
            //arrange
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

            var controller = new WorkHistoryController(mediator.Object)
            {
                Url = mockUrlHelper.Object
            };

            var actual = controller.Get(applicationId) as ViewResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.Model.Should().NotBeNull();

            var actualModel = actual?.Model as AddWorkHistoryViewModel;
            actualModel?.ApplicationId.Should().Be(applicationId);
        }
    }
}
