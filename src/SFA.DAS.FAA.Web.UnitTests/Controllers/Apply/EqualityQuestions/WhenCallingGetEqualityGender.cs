using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Extensions;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions
{
    [TestFixture]
    public class WhenCallingGetEqualityGender
    {
        private static readonly string Key = $"{CacheKeys.EqualityQuestionsDataProtectionKey}-{CacheKeys.EqualityQuestions}";

        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            Guid applicationId,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var cacheKey = string.Format($"{Key}", candidateId);
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            cacheStorageService
                .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
                .ReturnsAsync((EqualityQuestionsModel)null!);

            var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
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

            var actual = await controller.Gender(applicationId) as ViewResult;
            var actualModel = actual!.Model.As<EqualityQuestionsGenderViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.ApplicationId.Should().Be(applicationId);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_Cached_Value_View_Is_Returned(
            Guid applicationId,
            Guid candidateId,
            EqualityQuestionsModel model,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var cacheKey = string.Format($"{Key}", candidateId);
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

            cacheStorageService
                .Setup(x => x.Get<EqualityQuestionsModel>(cacheKey))
                .ReturnsAsync(model);

            var controller = new EqualityQuestionsController(mediator.Object, cacheStorageService.Object)
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

            var actual = await controller.Gender(applicationId) as ViewResult;
            var actualModel = actual!.Model.As<EqualityQuestionsGenderViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.ApplicationId.Should().Be(model.ApplicationId);
            }
        }
    }
}