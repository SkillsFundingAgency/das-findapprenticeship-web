using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using Microsoft.AspNetCore.Http;
using SFA.DAS.FAA.Web.AppStart;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using System;


namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    public class WhenPostingDeleteAJob
    {

        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Sent_Successfully(
        Guid candidateId,
        DeleteJobViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] WorkHistoryController controller)    
         {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            await controller.PostDeleteJob(model);


            mediator.Verify(x => x.Send(It.Is<PostDeleteJobCommand>(c =>
                c.CandidateId == candidateId
                && c.ApplicationId == model.ApplicationId
                && c.JobId == model.JobId),
            CancellationToken.None), Times.Once);

        }

            [Test, MoqAutoData]
            public async Task Then_If_An_Exception_Is_Thrown_Then_ModelState_Is_Updated_And_View_Returned(
                Guid candidateId,
                DeleteJobViewModel model,
                [Frozen] Mock<IMediator> mediator,
                [Greedy] WorkHistoryController controller)
            {
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                    }
                };

                mediator.Setup(x => x.Send(It.IsAny<PostDeleteJobCommand>(),
                        CancellationToken.None))
                    .ThrowsAsync(new InvalidOperationException());

                var result = await controller.PostDeleteJob(model) as ViewResult;

                result.Should().NotBeNull();
                result.ViewName.Should().Be("~/Views/apply/workhistory/DeleteJob.cshtml");
                controller.ModelState.ContainsKey(nameof(DeleteJobViewModel)).Should().BeTrue();
                controller.ModelState[nameof(DeleteJobViewModel)].Errors.Should().Contain(e => e.ErrorMessage == "There's been a problem");
            }

            [Test, MoqAutoData]
            public async Task Then_If_No_Exception_Is_Thrown_Then_RedirectToRouteResult_Is_Returned(
                Guid candidateId,
                DeleteJobViewModel model,
                [Frozen] Mock<IMediator> mediator,
                [Greedy] WorkHistoryController controller)
            {
                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                    }
                };
                var result = await controller.PostDeleteJob(model) as RedirectToRouteResult;

                result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.ApplyApprenticeship.Jobs);
            }
    }
}
