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
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.EqualityQuestions
{
    [TestFixture]
    public class WhenCallingPostEqualityGender
    {
        [Test, MoqAutoData]
        public void And_ModelState_Is_InValid_Then_Return_View(
            Guid applicationId,
            Guid govIdentifier,
            EqualityQuestionsGenderViewModel viewModel,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {

            viewModel.IsGenderIdentifySameSexAtBirth = null;
            viewModel.Sex = null;

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
            controller.ModelState.AddModelError("test", "message");

            var actual = controller.Gender(applicationId, viewModel) as ViewResult;
            var actualModel = actual!.Model.As<EqualityQuestionsGenderViewModel>();

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Model.Should().NotBeNull();
                actualModel.Valid.Should().BeFalse();
                actualModel.ApplicationId.Should().Be(viewModel.ApplicationId);
            }
        }

        [Test, MoqAutoData]
        public void And_ModelState_Is_Valid_Then_Redirected_To_EqualityEthnicGroup(
                Guid applicationId,
                Guid govIdentifier,
                GenderIdentity gender,
                EqualityQuestionsGenderViewModel viewModel,
                [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            viewModel.Sex = gender.ToString();
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

            var actual = controller.Gender(applicationId, viewModel) as RedirectToRouteResult;

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual!.RouteName.Should().NotBeNull();
                actual.RouteName.Should().BeEquivalentTo(RouteNames.ApplyApprenticeship.EqualityQuestions.EqualityFlowEthnicGroup);
            }
        }
    }
}
