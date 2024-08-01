using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Error
{
    [TestFixture]
    public class ErrorControllerTest
    {

		[Test, MoqAutoData]
        public void Then_View_404_Is_Returned()
        {
	        var mockHttpContext = new Mock<HttpContext>();
	        var mockHttpResponse = new Mock<HttpResponse>();
	         
	        mockHttpContext.Setup(context => context.Response).Returns(mockHttpResponse.Object);

	        var controller = new ErrorController();
	        controller.ControllerContext.HttpContext = mockHttpContext.Object;

			var result = controller.PageNotFound() as ViewResult;

            result.Should().NotBeNull();
            mockHttpResponse.VerifySet(response => response.StatusCode = 200, Times.Once);
		}

        [Test, MoqAutoData]
        public void Then_View_500_Is_Returned([Greedy] ErrorController controller)
        {
            var result = controller.ApplicationError() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}
