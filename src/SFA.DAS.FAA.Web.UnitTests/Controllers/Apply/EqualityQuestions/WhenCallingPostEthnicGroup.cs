using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions
{
    [TestFixture]
    public class WhenCallingPostEthnicGroup
    {
        [Test, MoqAutoData]
        public async Task And_ModelState_Is_InValid_Then_Return_View(
            Guid applicationId,
            Guid govIdentifier,
            EqualityQuestionsEthnicGroupViewModel viewModel,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            viewModel.EthnicGroup = null;
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
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
            controller.ModelState.AddModelError("test", "message");

            var actual = await controller.EthnicGroup(applicationId, viewModel) as ViewResult;
            var actualModel = actual!.Model.As<EqualityQuestionsEthnicGroupViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.Valid.Should().BeFalse();
                actualModel.ApplicationId.Should().Be(viewModel.ApplicationId);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Cache_Is_InValid_Then_Redirected_To_EqualityFlowGender(
                Guid applicationId,
                Guid govIdentifier,
                EthnicGroup ethnicGroup,
                EqualityQuestionsEthnicGroupViewModel viewModel,
                [Frozen] Mock<IMediator> mediator,
                [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
            viewModel.EthnicGroup = ethnicGroup.ToString();
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            cacheStorageService.Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
                .ReturnsAsync((EqualityQuestionsModel?)null);

            var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
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

            var actual = await controller.EthnicGroup(applicationId, viewModel) as RedirectToRouteResult;

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.RouteName.Should().NotBeNull();
                actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowGender);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_Redirected_To_EthnicSubPage(
            Guid applicationId,
            Guid govIdentifier,
            EthnicGroup ethnicGroup,
            EqualityQuestionsModel equalityQuestionsModel,
            EqualityQuestionsEthnicGroupViewModel viewModel,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
            viewModel.EthnicGroup = ethnicGroup.ToString();
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            cacheStorageService.Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
                .ReturnsAsync(equalityQuestionsModel);

            var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
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

            var actual = await controller.EthnicGroup(applicationId, viewModel) as RedirectToRouteResult;
            var route = RouteNamesHelperService.GetEqualityFlowEthnicSubGroupRoute(ethnicGroup);

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.RouteName.Should().NotBeNull();
                actual.RouteName.Should().BeEquivalentTo(route);
            }
        }
    }
}
