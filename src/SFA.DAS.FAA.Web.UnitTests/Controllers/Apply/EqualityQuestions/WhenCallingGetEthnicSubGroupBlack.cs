using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions
{
    [TestFixture]
    public class WhenCallingGetEthnicSubGroupBlack
    {
        [Test, MoqAutoData]
        public void Then_View_Is_Returned(
            Guid applicationId,
            Guid govIdentifier,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            var controller = new EqualityQuestionsController(cacheStorageService.Object)
            {
                Url = mockUrlHelper.Object,
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            { new(ClaimTypes.NameIdentifier, govIdentifier.ToString()) }))
                    }
                }
            };

            var actual = controller.EthnicGroupBlack(applicationId) as ViewResult;
            var actualModel = actual!.Model.As<EqualityQuestionsEthnicSubGroupBlackViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.ApplicationId.Should().Be(applicationId);
            }
        }
    }
}