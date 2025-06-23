using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.WhatInterestsYou;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WhatInterestsYou
{
    [TestFixture]
    public class WhenPostingRequest
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Command_Is_Called_And_RedirectRoute_Returned(
            Guid candidateId,
            Guid applicationId,
            WhatInterestsYouViewModel request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WhatInterestsYouController controller)
        {
            //arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<UpdateWhatInterestsYouCommand>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            var actual = await controller.Post(applicationId, request) as RedirectToRouteResult;

            using var scope = new AssertionScope();
            actual.Should().NotBeNull();
            actual?.RouteName.Should().Be(RouteNames.Apply);
            actual?.RouteValues.Should().NotBeEmpty();
        }
        
        [Test, MoqAutoData]
        public async Task And_Autosaving_JsonResult_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            WhatInterestsYouViewModel request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] WhatInterestsYouController controller)
        {
            //arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new Claim(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator
                .Setup(x => x.Send(It.IsAny<UpdateWhatInterestsYouCommand>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);
            
            request.AutoSave = true;

            // act
            var actual = await controller.Post(applicationId, request) as JsonResult;

            // assert
            actual.Should().NotBeNull();
        }
    }
}
