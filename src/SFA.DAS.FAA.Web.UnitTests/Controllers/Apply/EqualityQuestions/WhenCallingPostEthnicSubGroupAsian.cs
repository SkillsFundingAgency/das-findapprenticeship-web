﻿using AutoFixture.NUnit3;
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
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using SFA.DAS.FAA.Web.AppStart;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions
{
    [TestFixture]
    public class WhenCallingPostEthnicSubGroupAsian
    {
        [Test, MoqAutoData]
        public async Task And_ModelState_Is_InValid_Then_Return_View(
            Guid applicationId,
            Guid govIdentifier,
            string modelStateError,
            EqualityQuestionsEthnicSubGroupAsianViewModel viewModel,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            viewModel.EthnicSubGroup = null;
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
                            { new(CustomClaims.CandidateId, govIdentifier.ToString()) }))
                    }
                }
            };
            controller.ModelState.AddModelError("test", modelStateError);

            var actual = await controller.EthnicGroupAsian(applicationId, viewModel) as ViewResult;
            var actualModel = actual!.Model.As<EqualityQuestionsEthnicSubGroupAsianViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.Valid.Should().BeFalse();
                actual.ViewName.Should().Be("~/Views/apply/EqualityQuestions/EthnicSubGroupAsian.cshtml");
                actualModel.Valid.Should().BeFalse();
                actualModel.ApplicationId.Should().Be(viewModel.ApplicationId);
                controller.ModelState.ContainsKey(nameof(EqualityQuestionsEthnicSubGroupAsianViewModel)).Should().BeFalse();
                controller.ModelState[nameof(EqualityQuestionsEthnicSubGroupAsianViewModel)]?.Errors.Should().Contain(e => e.ErrorMessage == modelStateError);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Cache_Is_InValid_Then_Redirected_To_EqualityFlowGender(
                Guid applicationId,
                Guid govIdentifier,
                EthnicSubGroup ethnicSubGroup,
                EqualityQuestionsEthnicSubGroupAsianViewModel viewModel,
                [Frozen] Mock<IMediator> mediator,
                [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
            viewModel.EthnicSubGroup = ethnicSubGroup.ToString();
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
                            { new(CustomClaims.CandidateId, govIdentifier.ToString()) }))
                    }
                }
            };

            var actual = await controller.EthnicGroupAsian(applicationId, viewModel) as RedirectToRouteResult;

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
            EthnicSubGroup ethnicSubGroup,
            EqualityQuestionsModel equalityQuestionsModel,
            EqualityQuestionsEthnicSubGroupAsianViewModel viewModel,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var cacheKey = string.Format($"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}", govIdentifier);
            viewModel.EthnicSubGroup = ethnicSubGroup.ToString();
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
                            { new(CustomClaims.CandidateId, govIdentifier.ToString()) }))
                    }
                }
            };

            var actual = await controller.EthnicGroupAsian(applicationId, viewModel) as RedirectToRouteResult;

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.RouteName.Should().NotBeNull();
                actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowSummary);
            }
        }
    }
}