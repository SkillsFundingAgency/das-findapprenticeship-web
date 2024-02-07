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


namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.WorkHistory
{
    public class WhenDeletingAJob
    {
        public class WhenDeletingJob
        {
            [Test, MoqAutoData]
            public async Task Then_The_Command_Is_Sent_Successfully(
                Guid candidateId,
                Guid applicationId,
                Guid jobId,
                [Frozen] Mock<IMediator> mediator,
                [Greedy] WorkHistoryController controller)
            {
                var command = new DeleteJobCommand
                {
                    CandidateId = candidateId,
                    ApplicationId = applicationId,
                    JobId = jobId
                };

                await controller.DeleteJob(applicationId, candidateId, jobId);

                mediator.Verify(x => x.Send(It.Is<DeleteJobCommand>(c =>
                        c.CandidateId == candidateId
                        && c.ApplicationId == applicationId
                        && c.JobId == jobId),
                    CancellationToken.None), Times.Once);
            }

            [Test, MoqAutoData]
            public async Task Then_If_An_Exception_Is_Thrown_Then_ModelState_Is_Updated_And_View_Returned(
                Guid candidateId,
                Guid applicationId,
                Guid jobId,
                [Frozen] Mock<IMediator> mediator,
                [Greedy] WorkHistoryController controller)
            {
                mediator.Setup(x => x.Send(It.IsAny<DeleteJobCommand>(),
                        CancellationToken.None))
                    .ThrowsAsync(new InvalidOperationException());

                var result = await controller.DeleteJob(applicationId, candidateId, jobId) as ViewResult;

                result.Should().NotBeNull();
                result.ViewName.Should().Be("~/Views/apply/workhistory/DeleteJob.cshtml");
                controller.ModelState.ContainsKey(nameof(DeleteJobViewModel)).Should().BeTrue();
                controller.ModelState[nameof(DeleteJobViewModel)].Errors.Should().Contain(e => e.ErrorMessage == "There's been a problem");
            }

            [Test, MoqAutoData]
            public async Task Then_If_No_Exception_Is_Thrown_Then_RedirectToRouteResult_Is_Returned(
                Guid candidateId,
                Guid applicationId,
                Guid jobId,
                [Frozen] Mock<IMediator> mediator,
                [Greedy] WorkHistoryController controller)
            {
                var result = await controller.DeleteJob(applicationId, candidateId, jobId) as RedirectToRouteResult;

                result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.ApplyApprenticeship.JobsSummary);
            }
        }
    }
}
