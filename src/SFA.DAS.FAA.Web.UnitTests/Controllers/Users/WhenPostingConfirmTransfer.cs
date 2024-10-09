using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.MigrateData;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using SFA.DAS.GovUK.Auth.Services;
using Microsoft.Extensions.Options;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    public class WhenPostingConfirmTransfer
    {
        [Test, MoqAutoData]
        public async Task Then_Cache_Is_Empty_Redirect_Returned(
            string legacyEmail,
            string email,
            Guid candidateId,
            ConfirmTransferViewModel viewModel,
            [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var controller = new UserController(mediator.Object, cacheStorageService.Object, Mock.Of<IConfiguration>(), faaConfig.Object, Mock.Of<IOidcService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        {
                            new(CustomClaims.CandidateId, candidateId.ToString()),
                            new(ClaimTypes.Email, email)
                        }))
                    }
                }
            };

            var cacheKey = string.Format($"{candidateId}-{CacheKeys.LegacyEmail}");
            cacheStorageService
                .Setup(x => x.Get<string>(cacheKey))
                .ReturnsAsync((string)null!);

            var result = await controller.ConfirmDataTransfer(viewModel) as RedirectToRouteResult;
            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.ServiceStartDefault);
        }

        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned(
            string legacyEmail,
            string email,
            Guid candidateId,
            ConfirmTransferViewModel viewModel,
            [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var controller = new UserController(mediator.Object, cacheStorageService.Object, Mock.Of<IConfiguration>(), faaConfig.Object, Mock.Of<IOidcService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        {
                            new(CustomClaims.CandidateId, candidateId.ToString()),
                            new(ClaimTypes.Email, email)
                        }))
                    }
                }
            };

            var cacheKey = string.Format($"{candidateId}-{CacheKeys.LegacyEmail}");
            cacheStorageService
                .Setup(x => x.Get<string>(cacheKey))
                .ReturnsAsync(legacyEmail);

            mediator.Setup(x => x.Send(It.Is<MigrateDataTransferCommand>(x => x.CandidateId == candidateId && x.EmailAddress == email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            var result = await controller.ConfirmDataTransfer(viewModel) as RedirectToRouteResult;
            result!.RouteName.Should().Be(RouteNames.FinishAccountSetup);
        }
    }
}
