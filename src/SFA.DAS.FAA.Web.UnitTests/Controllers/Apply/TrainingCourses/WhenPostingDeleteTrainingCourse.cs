using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.DeleteTrainingCourse;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers.Apply;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Apply.TrainingCourses
{
    public class WhenPostingDeleteTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Sent_Successfully(
            Guid candidateId,
            DeleteTrainingCourseViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] TrainingCoursesController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            await controller.Delete(model);


            mediator.Verify(x => x.Send(It.Is<DeleteTrainingCourseCommand>(c =>
                c.CandidateId == candidateId
                && c.ApplicationId == model.ApplicationId
                && c.TrainingCourseId == model.TrainingCourseId),
            CancellationToken.None), Times.Once);

        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_ModelState_Is_Updated_And_View_Returned(
            Guid candidateId,
            DeleteTrainingCourseViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] TrainingCoursesController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<DeleteTrainingCourseCommand>(),
                    CancellationToken.None))
                .ThrowsAsync(new InvalidOperationException());

            var result = await controller.Delete(model) as ViewResult;

            result.Should().NotBeNull();
            result.ViewName.Should().Be("/Views/apply/trainingcourses/DeleteTrainingCourse.cshtml");
            controller.ModelState.ContainsKey(nameof(DeleteTrainingCourseViewModel)).Should().BeTrue();
            controller.ModelState[nameof(DeleteTrainingCourseViewModel)].Errors.Should().Contain(e => e.ErrorMessage == "There's been a problem");
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Exception_Is_Thrown_Then_RedirectToRouteResult_Is_Returned(
            Guid candidateId,
            DeleteTrainingCourseViewModel model,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] TrainingCoursesController controller)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                { new(CustomClaims.CandidateId, candidateId.ToString()) }))
                }
            };
            var result = await controller.Delete(model) as RedirectToRouteResult;

            result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.ApplyApprenticeship.TrainingCourses);
        }
    }
}
