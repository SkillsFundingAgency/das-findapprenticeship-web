using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetSignIntoYourOldAccount;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenPostingSignIntoYourOldAccount
    {
        [Test, MoqAutoData]
        public async Task Then_When_The_Credentials_Are_Invalid_Then_View_Is_Returned_With_Error_In_ModelState(
            SignInToYourOldAccountViewModel viewModel,
            GetSignIntoYourOldAccountQueryResult queryResult,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UserController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            };

            queryResult.IsValid = false;
            mediator.Setup(x =>
                    x.Send(It.Is<GetSignIntoYourOldAccountQuery>(q =>
                        q.CandidateId == candidateId && q.Email == viewModel.Email && q.Password == viewModel.Password), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var result = await controller.SignInToYourOldAccount(viewModel);

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            controller.ModelState.ContainsKey("Password").Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task Then_When_The_Credentials_Are_Valid_Then_LegacyEmail_Is_Stored_In_Cache(
            SignInToYourOldAccountViewModel viewModel,
            GetSignIntoYourOldAccountQueryResult queryResult,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Greedy] UserController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            };

            queryResult.IsValid = true;

            mediator.Setup(x =>
                    x.Send(It.Is<GetSignIntoYourOldAccountQuery>(q =>
                        q.CandidateId == candidateId && q.Email == viewModel.Email && q.Password == viewModel.Password), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var result = await controller.SignInToYourOldAccount(viewModel);

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToRouteResult>();

            cacheStorageService.Verify(x => x.Set(It.Is<string>(key => key == $"{candidateId}-{CacheKeys.LegacyEmail}"), It.Is<string>(v => v == viewModel.Email), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}
